function getExcelExportProperties(filename, header, columnCount) {
    return {
        header: {
            headerRows: 7,
            rows: [
                {
                    index: 1,
                    cells: [

                        { index: 1, colSpan: columnCount, value: header, style: { fontColor: '#C25050', fontSize: 25, hAlign: 'Center', bold: true } }
                    ]
                //},
                //{
                //    index: 3,
                //    cells: [
                //        { index: 1, colSpan: 2, value: 'Adventure Traders', style: { fontColor: '#C67878', fontSize: 15, bold: true } },
                //        { index: 4, value: 'INVOICE NUMBER', style: { fontColor: '#C67878', bold: true } },
                //        { index: 5, value: 'DATE', style: { fontColor: '#C67878', bold: true }, width: 150 }
                //    ]
                //},
                //{
                //    index: 4,
                //    cells: [
                //        { index: 1, colSpan: 2, value: '2501 Aerial Center Parkway' },
                //        { index: 4, value: 2034 },
                //        { index: 5, value: 'date', width: 150 }
                //    ]
                //},
                //{
                //    index: 5,
                //    cells: [
                //        { index: 1, colSpan: 2, value: 'Tel +1 888.936.8638 Fax +1 919.573.0306' },
                //        { index: 4, value: 'CUSOTMER ID', style: { fontColor: '#C67878', bold: true } },
                //        { index: 5, value: 'TERMS', width: 150, style: { fontColor: '#C67878', bold: true } }
                //    ]
                //},
                //{
                //    index: 6,
                //    cells: [
                //        { index: 4, value: 564 },
                //        { index: 5, value: 'Net 30 days', width: 150 }
                //    ]
                }
            ]
        },

        //footer: {
        //    footerRows: 5,
        //    rows: [

        //        { cells: [{ colSpan: 6, value: 'Thank you for your business!', style: { fontColor: '#C67878', hAlign: 'Center', bold: true } }] },
        //        { cells: [{ colSpan: 6, value: '!Visit Again!', style: { fontColor: '#C67878', hAlign: 'Center', bold: true } }] }
        //    ]
        //},

        fileName: filename
    };
}