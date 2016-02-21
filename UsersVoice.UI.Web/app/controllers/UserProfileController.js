(function (module) {

    var ctrl = function($scope, $routeParams, UsersService, IdeasService, IdeaCommentsService) {
        var instance = this,
            isLoading = false,
            isLoadingIdeas = false,
            isLoadingComments = false;

        this.read = function() {
            if (!$routeParams.userId) {
                return;
            }

            isLoading = true;

            UsersService.details($routeParams.userId)
                .then(function(result) {
                    if (!result || !result.data) {
                        return;
                    }
                    $scope.model = result.data;
                    instance.readIdeas();
                    instance.readComments();
                }).then(function() {
                    isLoading = false;
                });
        };

        this.readIdeas = function() {
            if (!$scope.model || !$scope.model.Id) {
                return;
            }

            isLoadingIdeas = true;

            IdeasService.archive({
                authorId: $scope.model.Id,
                sortBy: 'CreationDate',
                sortDirection: 'DESC',
                pageSize: 10
            }).then(function(result) {
                if (!result || !result.data || !result.data.Items) {
                    return;
                }
                $scope.ideas = result.data.Items;
            }).then(function() {
                isLoadingIdeas = false;
            });
        };

        this.readComments = function() {
            if (!$scope.model || !$scope.model.Id) {
                return;
            }

            isLoadingComments = true;

            IdeaCommentsService.archive({
                authorId: $scope.model.Id,
                sortBy: 'CreationDate',
                sortDirection: 'DESC',
                pageSize: 10
            }).then(function(result) {
                if (!result || !result.data || !result.data.Items) {
                    return;
                }
                $scope.comments = result.data.Items;
            }).then(function() {
                isLoadingComments = false;
            });
        };

        this.initialize = function() {
            instance.read();
        };

        $scope.isLoading = function() {
            return isLoading;
        };
        $scope.isLoadingIdeas = function() {
            return isLoadingIdeas;
        };
        $scope.isLoadingComments = function() {
            return isLoadingComments;
        };

        $scope.tabs = [
            { dest: '#latestIdeas', text: 'Latest Ideas' },
            { dest: '#latestComments', text: 'Latest Comments' }
        ];

        this.initialize();
    };

    module.controller("UserProfileController", ['$scope', '$routeParams', 'UsersService', 'IdeasService', 'IdeaCommentsService', ctrl]);

})(angular.module("userVoice"));