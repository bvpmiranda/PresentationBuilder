angular.module('PresentationBuilder', [])

.controller('SignInController', function ($scope)
{
	$scope.Email;
	$scope.Password;

	$scope.signIn = function ()
	{
		var headers = {};
		headers['__RequestVerificationToken'] = csrfToken;

		$.ajax({
			type: "POST",
			url: baseUrl + 'Account/Login/',
			contentType: false,
			processData: false,
			data: {
				model: {
					Email: $scope.Email,
					Password: $scope.Password,
					RememberMe: true,
					__RequestVerificationToken: csrfToken
				}, returnUrl: ''
			},
			headers: headers,
			success: function (result)
			{
				result = jQuery.parseJSON(result);

				if (result.uploadStatus === uploadStatus.success)
				{
					Presentations.fileUpload = Presentations.fileUpload.replaceWith(Presentations.fileUpload.val('').clone(true));

					Navigation.navigate('Presentations/Presentation/' + result.data.PresentationId, result.data.Name)
				}
				else
				{
					alert('Erro: ' + result.message);
				}
			},
			error: function (xhr, status, p3, p4)
			{
				var err = "Error " + " " + status + " " + p3 + " " + p4;
				if (xhr.responseText && xhr.responseText[0] == "{")
					err = JSON.parse(xhr.responseText).Message;
				console.log(err);
			}
		});
	}
});