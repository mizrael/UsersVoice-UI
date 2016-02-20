(function (module) {
    var svc = function($http) {
        var archive = function(filters) {
                return $http({
                    method: 'GET',
                    url: '/api/areas/',
                    params: filters,
                    dataType: "json"
                });
            },
            ideas = function (areaId, pageSize, page) {
                if (!areaId) {
                    return;
                }
                var filters = {
                    pageSize: pageSize || 20,
                    page: page || 0
                };
                return $http({
                    method: 'GET',
                    url: "/api/areas/" + areaId + "/ideas",
                    params: filters,
                    dataType: "json"
                });
            },
            details = function(id) {
                return $http({
                    method: 'GET',
                    url: "/api/areas/" + id + '?_t=' + new Date().getTime(),
                    cache: false
                });
            };

        return {
            archive: archive,
            details: details,
            ideas: ideas
        };
    };

    module.service("AreasService", ['$http', svc]);
})(angular.module("userVoice"));