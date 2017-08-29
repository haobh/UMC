/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function () {
    angular.module('umc.menugroup', ['umc.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('menugroup', {
            url: "/menugroup",
            templateUrl: "/app/components/menugroup/menuGroupListView.html",
            controller: "menuGroupListController"
        });
    }
})();