using Fhi.HelseId.Web.Hpr;
using Fhi.HelseId.Web.Hpr.Core;

namespace dotnet_new_angular.HelseId
{
    public class HprApprovals : GodkjenteHprKategoriListe
    {
        public HprApprovals()
        {
            Add(Kodekonstanter.OId9060Lege);
        }
    }
}