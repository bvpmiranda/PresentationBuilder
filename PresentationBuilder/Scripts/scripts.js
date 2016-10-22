var baseUrl;
var csrfToken;

block = function (element, options)
{
	if (options === undefined || options === null)
	{
		options = {};
	}

	if (element === undefined || element === null)
	{
		$.blockUI(options);
	}
	else
	{
		element.block(options);
	}
}

unblock = function (element)
{
	if (element === undefined)
	{
		$.unblockUI();
	}
	else
	{
		element.unblock();
	}
}