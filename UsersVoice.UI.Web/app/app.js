angular.module('userVoice', ["ngRoute"])
    .config([
        '$routeProvider',
        function($routeProvider) {
            $routeProvider.
                when('/', {
                    templateUrl: '/app/views/Main.html'
                })
                .when('/areas/:areaId/create', {
                    templateUrl: '/app/views/SubmitIdea.html',
                    controller: 'SubmitIdeaDetailController'
                })
                .when('/areas/:areaId', {
                    templateUrl: '/app/views/AreaDetail.html',
                    controller: 'AreaDetailController'
                })
                .when('/ideas/:ideaId', {
                    templateUrl: '/app/views/IdeaDetail.html',
                    controller: 'IdeaDetailController'
                })
                .when('/users/:userId', {
                    templateUrl: '/app/views/UserProfile.html',
                    controller: 'UserProfileController'
                })
                .when("/login", {
                    templateUrl: "/app/views/Login.html",
                    controller: "LoginController"
                });

        }
    ]);