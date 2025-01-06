var app = angular.module("MyApp", ['ngTable', 'angucomplete-alt']);

app.controller("LiveCtrl", function ($scope, $http, $filter) {
    $scope.Init = function () {
        angular.element('#loading').hide();
    }

    $scope.HideLoader = function () {
        angular.element('#loading').hide();
    };

    $scope.showloader = function () {
        angular.element('#loading').show();
    };
});

app.controller("GetOPO", function ($scope, $http, $filter, NgTableParams) {
    $scope.GetOPOInit = function () {
        $scope.showloader();
        $http.get("/Dashboard/GetOPOData").then(function (res) {
            if (res.data.length != 0) {
                $scope.OpenPO2 = new NgTableParams({}, { dataset: res.data });
                $scope.OpenPO = res.data;
                $scope.HideLoader();
            }
            else {
                $scope.HideLoader();
                alert("You Dot't have any open PO !!");
            }
        });

        $scope.HideLoader = function () {
            angular.element('#loading').hide();
        };

        $scope.showloader = function () {
            angular.element('#loading').show();
        };

        $scope.$watch('dayData', function (newVal) {
            if (newVal && newVal.length) {
                initializeDayChart();
            }
        });

        function initializeDayChart() {
            var ctx = document.getElementById('day_chart').getContext('2d');

            // Check if dayData is defined and contains data
            if (!$scope.dayData || !$scope.dayData.length) {
                console.error('dayData is empty or undefined');
                return;
            }

            // Log dayData for debugging purposes
            console.log($scope.dayData);

            // Format the day data for the chart
            var days = $scope.dayData.map(item => item.Day);
            var companyCounts = $scope.dayData.map(item => item.Company_Count);
            var onSiteCounts = $scope.dayData.map(item => item.OnSite_Count);
            var otherCounts = $scope.dayData.map(item => item.Other_Count);

            // Log the processed data
            console.log('Days:', days);
            console.log('Company Counts:', companyCounts);
            console.log('Onsite Counts:', onSiteCounts);
            console.log('Other Counts:', otherCounts);

            // Create the chart
            var dayChart = new Chart(ctx, {
                type: 'bar',  // You can change this to 'line' or 'radar' depending on your preference
                data: {
                    labels: days,  // X-axis labels will be the days
                    datasets: [{
                        label: 'Company',
                        data: companyCounts,
                        backgroundColor: '#7638ff',  // Blue
                        borderColor: '#fff',
                        borderWidth: 2
                    },
                    {
                        label: 'OnSite',
                        data: onSiteCounts,
                        backgroundColor: '#ff737b',  // Red
                        borderColor: '#fff',
                        borderWidth: 2
                    },
                    {
                        label: 'Other',
                        data: otherCounts,
                        backgroundColor: '#fda600',  // Yellow
                        borderColor: '#fff',
                        borderWidth: 2
                    }]
                },
                options: {
                    responsive: true,
                    scales: {
                        x: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: 'Date'
                            }
                        },
                        y: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: 'Count'
                            }
                        }
                    },
                    plugins: {
                        legend: {
                            position: 'top'
                        },
                        tooltip: {
                            callbacks: {
                                label: function (tooltipItem) {
                                    return tooltipItem.dataset.label + ': ' + tooltipItem.raw;
                                }
                            }
                        }
                    }
                }
            });
        }

        const colors = ['#7638ff', '#ff737b', '#fda600', '#1ec1b0', '#00bcd4', '#9c27b0'];

        $scope.$watch('departmentStats', function (newVal) {
            if (newVal && newVal.length) {
                initializeChart();  // Initialize the chart when departmentStats are loaded
            }
        });

        function initializeChart() {
            var ctx = document.getElementById('invoice_chart1').getContext('2d');
            var chart = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: $scope.departmentStats.map(stat => stat.EmpDep),
                    datasets: [{
                        label: 'Employee Distribution',
                        data: $scope.departmentStats.map(stat => stat.Percentage),
                        backgroundColor: colors,
                        borderColor: '#fff',
                        borderWidth: 2
                    }]
                },
                options: {
                    responsive: true,
                    cutout: '70%',
                    plugins: {
                        legend: {
                            position: 'top',

                        },
                        tooltip: {
                            callbacks: {
                                label: function (tooltipItem) {
                                    return tooltipItem.label + ': ' + tooltipItem.raw + '%';
                                }
                            }

                        }


                    }
                }
            });
        }
    };

    $scope.HideLoader = function () {
        angular.element('#loading').hide();
    };

    $scope.showloader = function () {
        angular.element('#loading').show();
    };

});

app.controller("UserCreate", function ($scope, $http, $filter, NgTableParams) {
    $scope.UserCreateInit = function () {
        $scope.User = {};
        $scope.OnlyName = true;
        $scope.NameAll = false;
        $scope.NameVendor = false;
        $scope.EmpName = false;
        $scope.btnName = "Submit";
        $scope.BlockStatusHide = true;
        $scope.hideItemGrp = false;
        $scope.UserDiss = false;
        $scope.EmpFields = true;
        $scope.VendorDepShow = false;
        $scope.OtherDepShow = true;
        $http.get('/api/WebAPI/InitData').then(function (res) {
            if (res.data != undefined) {
                $scope.Itmgrp = res.data.Itmgrp;
                $scope.branch = res.data.branch;
                $scope.Dep = res.data.Dep;
                $scope.HideLoader();
            }
            else {
                $scope.HideLoader();
                alert("You Dot't have any open PO !!");
            }
        });
    };
    $scope.ListofuserInit = function () {
        try {
            $scope.Find();
            $scope.User = {};
            $scope.OnlyName = true;
            $scope.NameAll = false;
            $scope.NameVendor = false;
            $scope.EmpName = false;
            $scope.btnName = "Submit";
            $scope.BlockStatusHide = true;
            $scope.hideItemGrp = false;
            $scope.UserDiss = false;
            $scope.EmpFields = true;
            $scope.VendorDepShow = false;
            $scope.OtherDepShow = true;
            $http.get('/api/WebAPI/InitData').then(function (res) {
                if (res.data != undefined) {
                    $scope.Itmgrp = res.data.Itmgrp;
                    $scope.branch = res.data.branch;
                    $scope.Dep = res.data.Dep;
                    $scope.HideLoader();
                }
                else {
                    $scope.HideLoader();
                    alert("You Dot't have any open PO !!");
                }
            });
        } catch (e) {
            console.log();
        }
    }
    $scope.checkAll = false;
    $scope.toggleAll = function () {
        $scope.checkAll = !$scope.checkAll;
        angular.forEach($scope.Itmgrp, function (item) {
            item.selected = $scope.checkAll;
        });
    };
    $scope.CreateUser = function () {
        $scope.Dep[3].selected;
        var validation = $scope.Vali1();
        var Passckeck = $scope.Checkpass();
        if (validation && Passckeck) {
            if ($scope.User.U_password.length >= 4) {
                if ($scope.User.U_userType != undefined && $scope.User.U_userType != '') {
                    if ($scope.User.U_isActive != undefined && $scope.User.U_isActive != '') {
                        if ($scope.User.U_userType === "Vendor" || ($scope.User.U_assignHead !== undefined && $scope.User.U_assignHead !== '')) {

                            if ($scope.User.U_departmentID != undefined && $scope.User.U_departmentID != '') {
                                var tem = $scope.Dep.filter(function (item) {
                                    return item.Code == $scope.User.U_departmentID;
                                });
                                if (tem.length > 0) {
                                    $scope.User.U_department = tem[0].Name;
                                }
                            }
                            if ($scope.User.U_userType === "Vendor") {
                                let departmentIDs = [];
                                let departmentNames = [];
                                for (var i = 0; i < $scope.Dep.length; i++) {
                                    if ($scope.Dep[i].selected) {
                                        departmentIDs.push($scope.Dep[i].Code);
                                        departmentNames.push($scope.Dep[i].Name);
                                    }
                                }
                                $scope.User.U_departmentID = departmentIDs.join(',');
                                $scope.User.U_department = departmentNames.join(',');
                            }

                            if ($scope.User.U_BranchID != undefined && $scope.User.U_BranchID != '') {
                                var tem2 = $scope.branch.filter(function (item) {
                                    return item.BPLId == $scope.User.U_BranchID;
                                });
                                if (tem2.length > 0) {
                                    $scope.User.U_BranchName = tem2[0].BPLName;
                                }
                            }
                            $scope.User.User_R = [];
                            for (var i = 0; i < $scope.Itmgrp.length; i++) {
                                if ($scope.Itmgrp[i].selected == true) {
                                    $scope.User.User_R.push($scope.Itmgrp[i])
                                }
                            }
                            if ($scope.btnName == "Submit") {
                                $http.post('/Dashboard/UserAdd', $scope.User).then(function (res) {
                                    if (res.data === "Ok") {
                                        alert("User Created !!");
                                        $scope.HideLoader();
                                        location.reload();
                                    }
                                    else {
                                        $scope.HideLoader();
                                        alert("User Creation Faild !!" + res.data);
                                    }
                                });
                            }
                            else if ($scope.btnName == "Update") {
                                if ($scope.User.U_Wrong_Attempts != "0")
                                    $scope.User.U_Wrong_Attempts = parseInt($scope.User.U_Wrong_Attempts);
                                else
                                    $scope.User.U_Wrong_Attempts = null;
                                $http.post('/Dashboard/UserUpdate', $scope.User).then(function (res) {
                                    if (res.data === "Update") {
                                        alert("User Update !!");
                                        $scope.HideLoader();
                                        location.reload();
                                    }
                                    else {
                                        $scope.HideLoader();
                                        alert("User Creation Faild !!" + res.data.Message);
                                    }
                                });
                            }
                        } else {
                            alert("Please Select First reporting header");
                        }

                    } else {
                        alert("Please Select Active Status !!");
                    }
                }
                else {
                    alert("Please Select User Type !!");
                }
            }
            else {
                alert("Your password should be more than 4 characters !!");
            }

        }
        else {
            if (validation === false)
                alert("Please fill User name !!");
            else
                alert("Please match the password with the confirm password!!");
        }
    };
    $scope.Vali1 = function () {
        if ($scope.User.U_userType === "Vendor") {
            if ($scope.User.U_userID === undefined || $scope.User.U_userID === '') {
                return false;
            }
            else {
                return true;
            }
        }
        else if ($scope.User.U_userType !== "Vendor") {
            if ($scope.User.U_userName === undefined || $scope.User.U_userName === '') {
                return false;
            }
            else {
                return true;
            }
        }
    };
    $scope.Find = function () {
        $http.get('/Dashboard/Find').then(function (res) {
            if (res.data != undefined) {
                $scope.Aluser = res.data;
                $scope.HideLoader();
            }
            else {
                $scope.HideLoader();
                alert("Error !!" + res.data.Message);
            }
        });
    };
    $scope.Edit = function (x) {
        var edt = {
            DocEntry: x.DocEntry,
            U_password: x.U_password
        };
        $http({
            method: 'GET',
            url: '/Dashboard/FindRow',
            params: edt
        }).then(function (res) {
            if (res.data != undefined) {
                $scope.GetHead();
                x.U_password = res.data.Decpass;
                if(x.U_userType == "Vendor") {
                    $scope.OnlyName = false;
                    $scope.NameAll = false;
                    $scope.EmpName = false;
                    $scope.NameVendor = true;
                    $scope.VendorDepShow = true;
                    $scope.OtherDepShow = false;
                    $scope.GetVendor();
                } else if(x.U_userType == "Employee") {
                    $scope.OnlyName = false;
                    $scope.NameAll = false;
                    $scope.EmpName = true;
                    $scope.EmpFields = true;
                    $scope.NameVendor = false;
                    $scope.VendorDepShow = false;
                    $scope.hideItemGrp = false;
                    $scope.OtherDepShow = true;
                }
                else {
                    $scope.OnlyName = false;
                    $scope.NameAll = true;
                    $scope.EmpName = false;
                    $scope.NameVendor = true;
                    $scope.VendorDepShow = false;
                    $scope.OtherDepShow = true;
                    $scope.hideItemGrp = false;
                }
                if (x.U_Wrong_Attempts == 3) {
                    $scope.BlockStatusHide = false;
                    $scope.User.U_Wrong_Attempts = "3";
                }
                $scope.User = x;
                if (x.U_userType == "Vendor") {
                    let X = x.U_departmentID;
                    let ARR = X.split(',').map(Number);
                    for (var j = 0; j < $scope.Dep.length; j++) {
                        $scope.Dep[j].selected = false;
                    }
                    for (var j = 0; j < $scope.Dep.length; j++) {
                        for (var i = 0; i < ARR.length; i++) {
                            if ($scope.Dep[j].Code == ARR[i]) {
                                $scope.Dep[j].selected = true;
                            }
                        }
                    }
                }
                $scope.UserDiss = true;
                angular.element("#VendorAngu_value").val(x.U_userID);
                $scope.User.U_userName = x.U_userName;
                angular.element("#HeadAngu_value").val(x.U_assignHead);
                $scope.User.U_assignHeadName = x.U_assignHeadName;
                angular.element("#HeadAngu2_value").val(x.U_assignHead2);
                $scope.User.U_AssignHeadName2 = x.U_AssignHeadName2;
                $scope.User.U_confirmPassword = x.U_password;
                $scope.btnName = "Update";

                for (var j = 0; j < $scope.Itmgrp.length; j++) {
                    $scope.Itmgrp[j].selected = false;
                }
                for (var j = 0; j < $scope.Itmgrp.length; j++) {
                    for (var i = 0; i < res.data.result.length; i++) {
                        if ($scope.Itmgrp[j].ItmsGrpCod == res.data.result[i].U_ItmsGrpCod) {
                            $scope.Itmgrp[j].selected = true;
                        }
                    }
                }
            } else {
                $scope.HideLoader();
                alert("Error !!" + res.data.Message);
            }
        }).catch(function (error) {
            $scope.HideLoader();
            alert("Request failed: " + error.message);
        });
    };
    $scope.AfterSelectedSOBP = function (selected) {
        try {
            $http.get('/Dashboard/CheckVendor?UserId=' + selected.originalObject.CardCode).then(function (res) {
                if (res.data == "NotOk") {
                    $scope.User.U_userID = '';
                    alert("This Vendor User Id already Created  !!");
                }
                else {
                    angular.element('#VendorAngu').val(selected.originalObject.CardCode);
                    $scope.User.U_userName = selected.originalObject.CardName;
                    $scope.User.U_userID = selected.originalObject.CardCode;
                }
            });
        }
        catch (ex) {
            console("Name not Found");
        }
    };
    $scope.AfterSelectedSOBP2 = function (selected) {
        try {
            $scope.FilteredHeadsForSecond = [];
            if (selected != undefined) {
                if (selected.originalObject.U_userID == $scope.User.U_assignHead2) {
                    $scope.User.U_assignHeadName = '';
                    $scope.User.U_assignHead = '';
                    alert("This reporting header already assigned !!");
                }
                else {
                    angular.element('#HeadAngu').val(selected.originalObject.U_userID);
                    $scope.User.U_assignHead = selected.originalObject.U_userID;
                    $scope.User.U_assignHeadName = selected.originalObject.U_userName;
                    for (var i = 0; i < $scope.Head.length; i++) {
                        if ($scope.Head[i].U_userID != selected.originalObject.U_userID) {
                            $scope.FilteredHeadsForSecond.push($scope.Head[i]);
                        }
                    }
                }
            }
            else {
                $scope.User.U_assignHeadName = '';
                $scope.User.U_assignHead = '';
                $scope.FilteredHeadsForSecond = $scope.Head;
            }
        }
        catch (ex) {
            console("Name not Found");
        }
    };
    $scope.AfterSelectedSOBP3 = function (selected) {
        try {
            if (selected == undefined) {
                $scope.User.U_AssignHeadName2 = '';
                $scope.User.U_assignHead2 = '';
            }
            angular.element('#HeadAngu2').val(selected.originalObject.U_userID);
            $scope.User.U_assignHead2 = selected.originalObject.U_userID;
            $scope.User.U_AssignHeadName2 = selected.originalObject.U_userName;

        }
        catch (ex) {
            console("Name not Found");
        }
    }
    $scope.AfterSelectedSOBP4 = function (selected) {
        try {
            if (selected == undefined) {
                $scope.User.U_userID = '';
                $scope.User.U_userName = '';
            }
            angular.element('#EmpAngu').val(selected.originalObject.EmpID);
            $scope.User.U_userID = selected.originalObject.EmpID;
            $scope.User.U_userName = selected.originalObject.EmpName;
            $scope.User.U_departmentID = String(selected.originalObject.EmpDepCode);
        }
        catch (ex) {
            console("Name not Found");
        }
    }
    $scope.Checkpass = function () {
        try {

            if ($scope.User.U_password != undefined && $scope.User.U_password != '') {
                if ($scope.User.U_password != $scope.User.U_confirmPassword) {
                    $scope.User.U_password = '';
                    $scope.User.U_confirmPassword = '';
                    alert("Your Password Not Match with ConfirmPassword");
                    return false;
                }
            }
            else {
                return false;
            }
            return true;
        }
        catch (ex) {
            console("Name not Found");
        }
    };
    $scope.GetVendor = function () {
        try {
            $scope.GetHead();
            if ($scope.User.U_userType == "Vendor") {
                $scope.hideItemGrp = true;
                $http.get('/api/WebAPI/GetVendor').then(function (res) {
                    if (res.data != undefined) {
                        $scope.Vendor = res.data.Vendor;
                        $scope.NameVendor = true;
                        $scope.OnlyName = false;
                        $scope.NameAll = false;
                        $scope.EmpName = false;
                        $scope.VendorDepShow = true;
                        $scope.HideLoader();
                    }
                    else {
                        $scope.HideLoader();
                        alert("Error !!" + res.data.Message);
                    }
                });

            }
            else if ($scope.User.U_userType == "Employee") {
                $http.get('/api/WebAPI/GetEmp').then(function (res) {
                    if (res.data != undefined) {
                        $scope.Emp = res.data.Emp;
                        $scope.NameVendor = false;
                        $scope.OnlyName = false;
                        $scope.NameAll = false;
                        $scope.EmpName = true;
                        $scope.VendorDepShow = false;
                        $scope.hideItemGrp = false;
                        $scope.HideLoader();
                    }
                    else {
                        $scope.HideLoader();
                        alert("Error !!" + res.data.Message);
                    }
                });
            }
            else {
                $scope.OnlyName = true;
                $scope.NameVendor = false;
                $scope.NameAll = false;
                $scope.hideItemGrp = false;
                $scope.VendorDepShow = false;
                $scope.hideItemGrp = false;
            }

        }
        catch (ex) {
            console("Name not Found");
        }
    }
    $scope.GetHead = function () {
        try {
            $http.get('/api/WebAPI/GetassHed?UserType=' + $scope.User.U_userType).then(function (res) {
                if (res.data != undefined) {
                    $scope.Head = res.data;
                }
                else {
                    $scope.HideLoader();
                    alert("Error !!" + res.data.Message);
                }
            });
        } catch (e) {
            console.log(e);
        }
    };
});

app.controller("AllVendorDTL", function ($scope, $http, $filter, NgTableParams) {
    $scope.AllVendorInit = function () {
        $http.get('/api/WebAPI/AllVendor').then(function (res) {
            if (res.data != undefined) {
                $scope.AllVendor = res.data.Allvendor;
                $scope.HideLoader();
            }
            else {
                $scope.HideLoader();
                alert("You Don't have any open PO !!");
            }
        });
    };
    $scope.VendorRowDtl = function (x) {
        $http.get('/api/WebAPI/VendorRowDtl?DocNum=' + x.DocNum + '&CardCode=' + x.CardCode + '&DocStatus=' + x.DocStatus).then(function (res) {
            if (res.data != undefined) {
                $scope.AllVendorRow = res.data.AllVendorRow;
                $scope.HideLoader();
            }
            else {
                $scope.HideLoader();
                alert("You Dot't have any open PO !!");
            }
        });
    };
});

app.controller("Aging", function ($scope, $http, $filter, NgTableParams) {
    $scope.AgingInit = function () {
        $http.get('/api/WebAPI/AllVendorAging').then(function (res) {
            if (res.data != undefined) {
                $scope.Agine = res.data.Agine;
                $scope.HideLoader();
            }
            else {
                $scope.HideLoader();
                alert("You Don't have any open PO !!");
            }
        });
    };
});
