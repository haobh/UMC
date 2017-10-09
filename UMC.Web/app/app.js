/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('umc',
        ['umc.menugroup',
         'umc.menu',
         'umc.footer',
         'umc.common',
         'umc.application_users',
         'umc.application_groups',
         'umc.application_roles'])
        .config(config)
        .config(configAuthentication);//Kiểm tra xem đã Authen chưa

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('base', {
                url: '',
                templateUrl: '/app/shared/views/baseView.html',
                abstract: true
            }).state('login', {
                url: "/login",
                templateUrl: "/app/components/login/loginView.html",
                controller: "loginController"
            }).state('home', {
                url: "/admin",
                parent: 'base',  //Gọi phương thức baseView, giống renderPage
                templateUrl: "/app/components/home/homeView.html",
                controller: "homeController"
            });
        $urlRouterProvider.otherwise('/login');
    }

    //Kiểm tra xem đã Authen chưa
    function configAuthentication($httpProvider) {
        $httpProvider.interceptors.push(function ($q, $location) {
            return {
                request: function (config) {
                    return config;
                },
                requestError: function (rejection) {
                    return $q.reject(rejection);
                },
                response: function (response) {
                    if (response.status == "401") {
                        $location.path('/login');
                    }
                    //the same response/modified/or a new one need to be returned.
                    return response;
                },
                responseError: function (rejection) {
                    if (rejection.status == "401") {
                        $location.path('/login');
                    }
                    return $q.reject(rejection);
                }
            };
        });
    }
})();