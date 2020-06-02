if (!window) {

    var window = exports.window = {};
}
windows.samplesList = getSampleData();
//window.samplesList = [
//    {
//        "name": "Master",
//        "directory": "Master",
//        "category": "HRM",
//        "type": "Preview",
//        "samples": [
//            {
//                "url": "Cmpy",
//                "name": "Company",
//                "category": "Master",
//                "uid": "A00000",
//                "order": 0,
//                "component": "Master",
//                "dir": "Master",
//                "hidden": false,
//                "parentId": "A100"
//            },
//            {
//                "url": "Dept",
//                "name": "Department",
//                "category": "Master",
//                "uid": "A00001",
//                "order": 0,
//                "component": "Master",
//                "dir": "Master",
//                "hidden": false,
//                "parentId": "A100"
//            },
//            {
//                "url": "Posi",
//                "name": "Position",
//                "category": "Master",
//                "uid": "A00002",
//                "order": 0,
//                "component": "Master",
//                "dir": "Master",
//                "hidden": false,
//                "parentId": "A100"
//            },
//            {
//                "url": "Leav",
//                "name": "Leave",
//                "category": "Master",
//                "uid": "A00003",
//                "order": 0,
//                "component": "Master",
//                "dir": "Master",
//                "hidden": false,
//                "parentId": "A100"
//            },
//            {
//                "url": "Grad",
//                "name": "Grading",
//                "category": "Master",
//                "uid": "A00004",
//                "order": 0,
//                "component": "Master",
//                "dir": "Master",
//                "hidden": false,
//                "parentId": "A100"
//            }
//        ],
//        "order": 0,
//        "uid": "A100"
//    },
//    {
//        "name": "Staff",
//        "directory": "Staff",
//        "category": "HRM",
//        //"type": "Preview",
//        "samples": [
//            {
//                "url": "OverviewList",
//                "name": "Overview List",
//                "category": "Staff",
//                "uid": "A10001",
//                "order": 0,
//                "component": "Staff",
//                "dir": "Staff",
//                "parentId": "A002"
//            },
//            {
//                "url": "FullDetail",
//                "name": "Staff Detail",
//                "category": "Staff",
//                "uid": "A10002",
//                "order": 0,
//                "component": "Staff",
//                "dir": "Staff",
//                "parentId": "A002"
//            },
//            {
//                "url": "Personal",
//                "name": "Personal Information",
//                "category": "Personal Information",
//                "uid": "A10003",
//                "order": 0,
//                "component": "Staff",
//                "dir": "Staff",
//                "hidden": true,
//                "parentId": "A002"
//            },
//            {
//                "url": "Salary",
//                "name": "Salary Information",
//                "category": "Staff",
//                "uid": "A10004",
//                "order": 0,
//                "component": "Staff",
//                "dir": "Staff",
//                "hidden" : true,
//                "parentId": "A002"
//            },
//            {
//                "url": "MPF",
//                "name": "MPF Information",
//                "category": "Staff",
//                "uid": "A10005",
//                "order": 0,
//                "component": "Staff",
//                "dir": "Staff",
//                "hidden": true,
//                "parentId": "A002"
//            },
//            {
//                "url": "Taxation",
//                "name": "Taxation Information",
//                "category": "Staff",
//                "uid": "A10006",
//                "order": 0,
//                "component": "Staff",
//                "dir": "Staff",
//                "hidden": true,
//                "parentId": "A002"
//            }
//        ],
//        "order": 1,
//        "uid": "A200"
//    }
//]
