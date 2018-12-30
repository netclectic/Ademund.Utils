using AgileObjects.AgileMapper.Extensions;
using System.IO;
using System.Xml;
using TidyManaged;

namespace Ademund.Utils
{
    public static class XmlUtils
	{
		public static XmlDocument TidyHtml(string htmlToTidy, string encoding="Utf8", TidyOptions options = null)
		{
			var encodingType = (EncodingType)System.Enum.Parse(typeof(EncodingType), encoding, true);

            if (options == null)
                options = new TidyOptions();

            using (Document doc = options.CreateDocumentFromString(htmlToTidy))
			{
				string result = doc.CleanAndRepair();
				string tidied = doc.Save();

				var xmlReaderSettings = new XmlReaderSettings()
				{
					XmlResolver = null,
					DtdProcessing = DtdProcessing.Ignore,
				};

				using (var reader = XmlReader.Create(new StringReader(tidied), xmlReaderSettings))
				{
					var xmlDoc = new XmlDocument();
					xmlDoc.Load(reader);
					return xmlDoc;
				}
			}
		}

		public static XmlDocument LoadFromXmlIgnoringDtd(string xml)
		{
			var xmlReaderSettings = new XmlReaderSettings()
			{
				XmlResolver = null,
				DtdProcessing = DtdProcessing.Ignore
			};

			using (var reader = XmlReader.Create(new StringReader(xml), xmlReaderSettings))
			{
				var xmlDoc = new XmlDocument();
				xmlDoc.Load(reader);
				return xmlDoc;
			}
		}

        public class TidyOptions
        {
            public AccessibilityCheckLevel AccessibilityCheckLevel { get; set; }
            public bool AddTidyMetaElement { get; set; }
            public bool AddVerticalSpace { get; set; }
            public bool AddXmlDeclaration { get; set; }
            public bool AddXmlSpacePreserve { get; set; }
            public bool AllowNumericCharacterReferences { get; set; }
            public bool AnchorAsName { get; set; }
            public bool AsciiEntities { get; set; }
            public SortStrategy AttributeSortType { get; set; }
            public bool ChangeXmlProcessingInstructions { get; set; }
            public EncodingType CharacterEncoding { get; set; }
            public bool CleanWord2000 { get; set; }
            public string CssPrefix { get; set; }
            public bool DecorateInferredUL { get; set; }
            public string DefaultAltText { get; set; }
            public DocTypeMode DocType { get; set; }
            public bool DropEmptyElems { get; set; }
            public bool DropEmptyParagraphs { get; set; }
            public bool DropFontTags { get; set; }
            public bool DropProprietaryAttributes { get; set; }
            public bool EncloseBlockText { get; set; }
            public bool EncloseBodyText { get; set; }
            public bool EnsureLiteralAttributes { get; set; }
            public string ErrorFile { get; set; }
            public bool EscapeCdata { get; set; }
            public bool FixAttributeUris { get; set; }
            public bool FixBadComments { get; set; }
            public bool FixUrlBackslashes { get; set; }
            public bool ForceOutput { get; set; }
            public bool IndentAttributes { get; set; }
            public AutoBool IndentBlockElements { get; set; }
            public bool IndentCdata { get; set; }
            public int IndentSpaces { get; set; }
            public EncodingType InputCharacterEncoding { get; set; }
            public bool JoinClasses { get; set; }
            public bool JoinStyles { get; set; }
            public bool KeepModificationTimestamp { get; set; }
            public bool LineBreakBeforeBR { get; set; }
            public bool LowerCaseLiterals { get; set; }
            public bool MakeBare { get; set; }
            public bool MakeClean { get; set; }
            public bool Markup { get; set; }
            public int MaximumErrors { get; set; }
            public AutoBool MergeDivs { get; set; }
            public AutoBool MergeSpans { get; set; }
            public string NewBlockLevelTags { get; set; }
            public string NewEmptyInlineTags { get; set; }
            public string NewInlineTags { get; set; }
            public NewlineType NewLine { get; set; }
            public string NewPreTags { get; set; }
            public AutoBool OutputBodyOnly { get; set; }
            public AutoBool OutputByteOrderMark { get; set; }
            public EncodingType OutputCharacterEncoding { get; set; }
            public string OutputFile { get; set; }
            public bool OutputHtml { get; set; }
            public bool OutputNumericEntities { get; set; }
            public bool OutputXhtml { get; set; }
            public bool OutputXml { get; set; }
            public bool PreserveEntities { get; set; }
            public bool PunctuationWrap { get; set; }
            public bool Quiet { get; set; }
            public bool QuoteAmpersands { get; set; }
            public bool QuoteMarks { get; set; }
            public bool QuoteNonBreakingSpaces { get; set; }
            public bool RemoveComments { get; set; }
            public bool RemoveEndTags { get; set; }
            public RepeatedAttributeMode RepeatedAttributeMode { get; set; }
            public bool ShowWarnings { get; set; }
            public int TabSize { get; set; }
            public bool UpperCaseAttributes { get; set; }
            public bool UpperCaseTags { get; set; }
            public bool UseColorNames { get; set; }
            public bool UseGnuEmacsErrorFormat { get; set; }
            public bool UseLogicalEmphasis { get; set; }
            public bool UseXmlParser { get; set; }
            public bool WrapAsp { get; set; }
            public int WrapAt { get; set; }
            public bool WrapAttributeValues { get; set; }
            public bool WrapJste { get; set; }
            public bool WrapPhp { get; set; }
            public bool WrapScriptLiterals { get; set; }
            public bool WrapSections { get; set; }
            public bool WriteBack { get; set; }

            public TidyOptions()
            {
                using (var defaultDoc = Document.FromString(""))
                {
                    defaultDoc.Map().OnTo(this);
                    ApplyDefaultProperties();
                }
            }

            public void ApplyDefaultProperties()
            {
                ShowWarnings = false;
                Quiet = true;
                OutputXhtml = true;
                DocType = DocTypeMode.Omit;

                AddTidyMetaElement = false;
                CharacterEncoding = (EncodingType)System.Enum.Parse(typeof(EncodingType), "Utf8", true);
                ForceOutput = true;
                MakeBare = false;

                AsciiEntities = true;
                DropEmptyElems = false;
                DropEmptyParagraphs = false;
                MergeDivs = AutoBool.No;
                MergeSpans = AutoBool.No;
                OutputNumericEntities = true;
                QuoteAmpersands = true;
                QuoteNonBreakingSpaces = true;
                WrapAt = 0;

                if (DefaultAltText == null)
                    DefaultAltText = string.Empty;
                if (CssPrefix == null)
                    CssPrefix = "c";
                if (NewPreTags == null)
                    NewPreTags = string.Empty;
                if (ErrorFile == null)
                    ErrorFile = string.Empty;
                if (OutputFile == null)
                    OutputFile = string.Empty;

                // add new html5 tags
                if (string.IsNullOrWhiteSpace(NewBlockLevelTags))
                    NewBlockLevelTags = "article aside audio details dialog figcaption figure footer header hgroup menutidy nav section source summary track video";
                if (string.IsNullOrWhiteSpace(NewEmptyInlineTags))
                    NewEmptyInlineTags = "command embed keygen source track wbr";
                if (string.IsNullOrWhiteSpace(NewInlineTags))
                    NewInlineTags = "canvas command data datalist embed keygen mark meter output progress time wbr";
            }

            internal Document CreateDocumentFromString(string htmlToTidy)
            {
                var document = Document.FromString(htmlToTidy);
                this.Map().OnTo(document);

                if (document.DefaultAltText == null)
                    document.DefaultAltText = string.Empty;
                if (document.CssPrefix == null)
                    document.CssPrefix = "c";
                if (document.NewPreTags == null)
                    document.NewPreTags = string.Empty;
                if (document.ErrorFile == null)
                    document.ErrorFile = string.Empty;
                if (document.OutputFile == null)
                    document.OutputFile = string.Empty;

                // add new html5 tags
                if (string.IsNullOrWhiteSpace(document.NewBlockLevelTags))
                    document.NewBlockLevelTags = "article aside audio details dialog figcaption figure footer header hgroup menutidy nav section source summary track video";
                if (string.IsNullOrWhiteSpace(document.NewEmptyInlineTags))
                    document.NewEmptyInlineTags = "command embed keygen source track wbr";
                if (string.IsNullOrWhiteSpace(document.NewInlineTags))
                    document.NewInlineTags = "canvas command data datalist embed keygen mark meter output progress time wbr";

                return document;
            }
        }
	}
}
