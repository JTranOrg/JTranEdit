using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;


namespace JTranEdit
{
	public interface IFoldingStrategy
	{
		IEnumerable<NewFolding> UpdateFoldings(TextDocument document);
		Preferences			    Preferences { get; set; }

		void CollapseAll();
		void ExpandAll();
	}

	public class JsonFoldingStrategies : IFoldingStrategy
	{
		private readonly List<IFoldingStrategy> _strategies = new List<IFoldingStrategy>();
		private readonly FoldingManager         _foldingManager;
		private readonly IComparer<NewFolding>  _comparer = new FoldingComparer();

		public JsonFoldingStrategies(TextEditor winEditor, Preferences preferences)
		{
			_foldingManager = FoldingManager.Install(winEditor.TextArea);

			_strategies.Add(new BraceFoldingStrategy(_foldingManager, winEditor) { OpeningBrace = '{', ClosingBrace = '}' } );
			_strategies.Add(new BraceFoldingStrategy(_foldingManager, winEditor) { OpeningBrace = '[', ClosingBrace = ']' } );

			this.Preferences = preferences;
		}

		public Preferences Preferences { get; set; }

		public IEnumerable<NewFolding> UpdateFoldings(TextDocument document)
		{
			var foldings = new List<NewFolding>();

			if(this.Preferences?.ShowOutlining ?? true)
			{ 
				foreach(var strategy in _strategies)
					foldings.AddRange(strategy.UpdateFoldings(document));

				foldings.Sort(_comparer);
		    }

			_foldingManager.UpdateFoldings(foldings, -1);

			return foldings;
        }

		public void CollapseAll()
		{
			if (_foldingManager.AllFoldings == null)
				return;

			foreach(var folding in _foldingManager.AllFoldings)
			{
				folding.IsFolded = true;
			}

			// Unfold the first fold (if any) to give a useful overview on content
			var foldSection = _foldingManager.GetNextFolding(0);

			if (foldSection != null)
				foldSection.IsFolded = false;       
	   }

		public void ExpandAll()
		{
			if (_foldingManager.AllFoldings == null)
				return;

			foreach(var folding in _foldingManager.AllFoldings)
			{
				folding.IsFolded = false;
			}     
	   }

        private class FoldingComparer : IComparer<NewFolding>
        {
            public int Compare([AllowNull] NewFolding x, [AllowNull] NewFolding y)
            {
                return x.StartOffset.CompareTo(y.StartOffset);
            }
        }
    }

	public class BraceFoldingStrategy : IFoldingStrategy
	{
		private readonly FoldingManager _foldingManager;

		public BraceFoldingStrategy(FoldingManager foldingManager, TextEditor winEditor)
		{
			_foldingManager = foldingManager;
                
			UpdateFoldings(winEditor.Document);
		}

		/// <summary>
		/// Gets/Sets the opening brace. The default value is '{'.
		/// </summary>
		public char OpeningBrace { get; set; } = '{';
		public Preferences Preferences { get; set; } 
		
		/// <summary>
		/// Gets/Sets the closing brace. The default value is '}'.
		/// </summary>
		public char ClosingBrace { get; set; } = '}';
		
		public IEnumerable<NewFolding> UpdateFoldings(TextDocument document)
		{
			int firstErrorOffset;
			return CreateNewFoldings(document, out firstErrorOffset);
		}
		
		/// <summary>
		/// Create <see cref="NewFolding"/>s for the specified document.
		/// </summary>
		public IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
		{
			firstErrorOffset = -1;
			return CreateNewFoldings(document);
		}
		
		/// <summary>
		/// Create <see cref="NewFolding"/>s for the specified document.
		/// </summary>
		public IEnumerable<NewFolding> CreateNewFoldings(ITextSource document)
		{
			var newFoldings       = new List<NewFolding>();
			var startOffsets      = new Stack<int>();
			var lastNewLineOffset = 0;
			var openingBrace      = this.OpeningBrace;
			var closingBrace      = this.ClosingBrace;

			for (int i = 0; i < document.TextLength; i++) 
			{
				char c = document.GetCharAt(i);

				if (c == openingBrace) 
				{
					startOffsets.Push(i);
				} 
				else if (c == closingBrace && startOffsets.Count > 0) 
				{
					int startOffset = startOffsets.Pop();
					// don't fold if opening and closing brace are on the same line
					if (startOffset < lastNewLineOffset) {
						newFoldings.Add(new NewFolding(startOffset, i + 1));
					}
				} else if (c == '\n' || c == '\r') 
				{
					lastNewLineOffset = i + 1;
				}
			}
			newFoldings.Sort((a,b) => a.StartOffset.CompareTo(b.StartOffset));

			return newFoldings;
		}

		
        public void CollapseAll()
        {
        }

        public void ExpandAll()
        {
        }
	}
}
