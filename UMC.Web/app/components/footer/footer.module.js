/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function () {
    angular.module('umc.footer', ['umc.common']).config(config);
    //DI, hoac tiem 2 bien vao
    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('footer', {
            url: "/footer",
            templateUrl: "/app/components/footer/footerListView.html",
            controller: "footerListController"
        });
    }
})();