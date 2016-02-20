(function (module) {

    var userAvatar = function() {
        return {
            restrict: 'E',
            replace: true,
            scope: {
                completeName: '@name',
                id: '@',
                cssClass:'@class'
            },
            controller: function ($scope) {
                $scope.url = ($scope.id) ? "/api/useravatar/" + $scope.id : "";
            },
            template: '<a href="#/users/{{id}}"><img ng-src="{{url}}" alt="{{completeName}}" class="{{cssClass}}"></a>'
        };
    }

    module.directive('userAvatar', userAvatar);

})(angular.module("userVoice"));
