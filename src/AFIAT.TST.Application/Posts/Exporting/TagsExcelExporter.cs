using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using AFIAT.TST.DataExporting.Excel.NPOI;
using AFIAT.TST.Posts.Dtos;
using AFIAT.TST.Dto;
using AFIAT.TST.Storage;

namespace AFIAT.TST.Posts.Exporting
{
    public class TagsExcelExporter : NpoiExcelExporterBase, ITagsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TagsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTagForViewDto> tags)
        {
            return CreateExcelPackage(
                "Tags.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Tags"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, 2, tags,
                        _ => _.Tag.Name
                        );

                });
        }
    }
}