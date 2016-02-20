(function (module) {
    var svc = function ($http) {
	    var archive = function (filters) {
	        return $http({
	            method: 'GET',
	            url: '/api/ideaComments/',
	            params: filters,
	            dataType: "json"
	        });
	    };

		return {
		    archive: archive
		};
	};

    module.service("IdeaCommentsService", ['$http', svc]);
})(angular.module("userVoice"));