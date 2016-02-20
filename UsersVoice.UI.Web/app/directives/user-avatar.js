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
            template: '<a href="#/users/{{id}}"><img src="/api/useravatar/{{id}}" alt="{{completeName}}" class="{{cssClass}}"></a>'
        };
    }

    module.directive('userAvatar', userAvatar);

})(angular.module("userVoice"));
