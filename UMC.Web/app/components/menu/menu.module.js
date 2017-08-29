/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function () {
    angular.module('umc.menu', ['umc.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('menu', {
            url: "/menu",
            templateUrl: "/app/components/menu/menuListView.html",
            controller: "menuListController"
        });
    }
})();