(function (module) {

    var ctrl = function($scope, $routeParams, AreasService, IdeasService) {
        var instance = this,
            isLoading = false,
            isLoadingIdeas = false;

        this.read = function() {
            if (!$routeParams.areaId) {
                return;
            }

            isLoading = true;

            AreasService.details($routeParams.areaId)
                .then(function(result) {
                    if (!result || !result.data) {
                        return;
                    }
                    $scope.model = result.data;
                    instance.readIdeas();
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

        this.initialize = function() {
            instance.read();
        };

        $scope.isLoading = function() {
            return isLoading;
        };
        $scope.isLoadingIdeas = function() {
            return isLoadingIdeas;
        };

        this.initialize();
    };

    module.controller("AreaDetailController", ['$scope', '$routeParams', 'AreasService', 'IdeasService', ctrl]);

})(angular.module("userVoice"));