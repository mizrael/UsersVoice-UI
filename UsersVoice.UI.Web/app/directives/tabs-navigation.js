(function (module) {

    var directive = function() {
        return {
            restrict: 'E',
            replace: true,
            scope: {
                items: '='
            },
            link: function (scope, element) {
                element.on('click', 'li a', function (e) {
                    e.preventDefault();
                    $(this).tab('show');
                });
            }, 
            templateUrl: '/app/views/templates/tabs-navigation.html'
        };
    }

    module.directive('tabsNavigation', directive);
})(angular.module("userVoice"));
