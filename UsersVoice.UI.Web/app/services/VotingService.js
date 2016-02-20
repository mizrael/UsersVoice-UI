(function (module) {
	var votingService = function ($rootScope, $q, $http, LoginService) {
	    var voteForIdea = function (ideaId, numberOfPoints) {
	        var isLogged = LoginService.isLogged();
	        if (!isLogged)
	            return $q.when(false);

			var user = LoginService.getUser();
			$http({
				method: 'POST',
				url: '/api/ideas/vote/',
				data: { IdeaId: ideaId, VoterId: user.Id, Points: numberOfPoints }
			}).then(function successCallback(response) {
				LoginService.setUser(response.data);
				$rootScope.$broadcast("userchanged", response.data);
			}, function errorCallback(response) {
				debugger;
			});

		};

	    var hasVoted = function (ideaId) {
	        if (!ideaId) {
	            return $q.when(false);
	        }
	           
			var isLogged = LoginService.isLogged();

			if (!isLogged) {
			    return $q.when(false);
			}

			var user = LoginService.getUser();

			return	$http({
				method: 'GET',
				url: '/api/ideas/' + ideaId + '/votes/' + user.Id
			});
		}

		return {
			voteForIdea: voteForIdea,
			hasVoted: hasVoted
		};
	};

	module.service("VotingService", ['$rootScope', '$q', '$http', 'LoginService', votingService]);
})(angular.module("userVoice"));