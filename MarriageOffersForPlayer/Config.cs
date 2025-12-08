using MCM.Abstractions.Base.Global;

namespace MarriageOffersForPlayer
{
    public class Configs : AttributeGlobalSettings<Configs>
    {
        public override string Id => "MarriageOffersForPlayer";

        public override string DisplayName => "Marriage Offers For Player";

        public override string FolderName => "MarriageOffersForPlayer";

        public override string FormatType => "json2";
    }
}