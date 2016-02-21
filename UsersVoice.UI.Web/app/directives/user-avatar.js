(function (module) {

    var userAvatar = function() {
        return {
            restrict: 'E',
            replace: true,
            scope: {
                getCompleteName: '&name',
                getId: '&id',
                cssClass: '@class'
            },
            controller: ['$scope', function ($scope) {
                var id = $scope.getId();
                if (id) {
                    $scope.imageUrl = "/api/useravatar/" + id;
                    $scope.detailsUrl = "#/users/" + id;
                }
               
            }],
            template: '<a href="{{detailsUrl}}" title="{{getCompleteName()}}"><img ng-src="{{imageUrl}}" alt="{{getCompleteName()}}" title="{{getCompleteName()}}" class="{{cssClass}}"></a>'
        };
    }

    module.directive('userAvatar', userAvatar);

})(angular.module("userVoice"));
