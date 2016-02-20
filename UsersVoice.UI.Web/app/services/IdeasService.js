(function (module) {
    var svc = function ($http) {
        var archive = function(filters) {
                return $http({
                    method: 'GET',
                    url: '/api/ideas/',
                    params: filters,
                    dataType: "json"
                });
            },
            details = function(id) {
                return $http({
                    method: 'GET',
                    url: "/api/ideas/" + id + '?_t=' + new Date().getTime(),
                    cache: false
                });
            };

		return {
		    archive: archive,
            details: details
		};
	};

    module.service("IdeasService", ['$http', svc]);
})(angular.module("userVoice"));