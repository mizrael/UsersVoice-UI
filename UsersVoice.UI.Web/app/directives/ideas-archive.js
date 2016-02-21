(function (module) {

    var directive = function() {
        return {
            restrict: 'E',
            replace: true,
            scope: {
                items: '='
            },
            controller: ['$scope', function($scope) {
                $scope.isLoading = function() {
                    return !$scope.items;
                }
            }],
            templateUrl: '/app/views/templates/ideas.html'
        };
    }

    module.directive('ideasArchive', directive);
})(angular.module("userVoice"));
