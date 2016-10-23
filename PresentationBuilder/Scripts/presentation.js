
var Presentations = {

	fileUpload: null,

	bind: function ()
	{
		Presentations.fileUpload.on('change', function (e)
		{
			var files = e.target.files;

			if (files.length > 0)
			{
				if (window.FormData !== undefined)
				{
					var data = new FormData();
					for (var x = 0; x < files.length; x++)
					{
						data.append("file" + x, files[x]);
					}

					$.ajax({
						type: "POST",
						url: baseUrl + 'Presentations/UploadZipAsync',
						contentType: false,
						processData: false,
						data: data,
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
				} else
				{
					alert('This browser is not supported');
				}
			}
		});
	},

	download: function (id)
	{
		if ($('#download-form').length < 1)
		{
			$('<form>').attr({
				method: 'POST',
				id: 'download-form',
				action: "api/PresentationsAPI/download/" + id
			}).appendTo('body');
		}
		else
		{
			$('#download-form').attr('action', "api/PresentationsAPI/download/" + id);
			$('#download-form').html('');
		}

		$('#download-form').submit();
	},

	delete: function (id)
	{
		block();

		$.ajax({
			type: "POST",
			url: baseUrl + "api/PresentationsAPI/delete/" + id,
			cache: false,
			contentType: 'application/json; charset=utf-8',
		}).success(function (data, textStatus, jqXHR)
		{
			unblock();

			if (data.isValid)
			{
				window.location.reload();
			}
			else
			{
				alert(data.messages[0]);
			}
			
		}).error(function (jqXHR, textStatus, errorThrown)
		{
			unblock();

			alert('There was an error deleting the presentation');
		});
	},

	deletePage: function (id)
	{
		block();

		$.ajax({
			type: "POST",
			url: baseUrl + "api/PresentationsAPI/deletePage/" + id,
			cache: false,
			contentType: 'application/json; charset=utf-8',
		}).success(function (data, textStatus, jqXHR)
		{
			unblock();

			if (data.isValid)
			{
				window.location.reload();
			}
			else
			{
				alert(data.messages[0]);
			}

		}).error(function (jqXHR, textStatus, errorThrown)
		{
			unblock();

			alert('There was an error deleting the presentation');
		});
	},

	upload: function (e)
	{
		$("#uploadZipForm").submit();
	}
}


