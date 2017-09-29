/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function () {
    angular.module('umc.menu', ['umc.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('menu', {
                url: "/menu",
                parent: 'base',
                templateUrl: "/app/components/menu/menuListView.html",
                controller: "menuListController"
            }).state('add_menu', {
                url: "/add_menu",
                parent: 'base',
                templateUrl: "/app/components/menu/menuAddView.html",
                controller: "menuAddController"
            }).state('edit_menu', {
                url: "/edit_menu/:id",
                parent: 'base',
                templateUrl: "/app/components/menu/menuEditView.html",
                controller: "menuEditController"
            });
    }
})();