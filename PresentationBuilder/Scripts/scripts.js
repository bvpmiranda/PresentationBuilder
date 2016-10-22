var baseUrl;
var csrfToken;

var adjustingLayout = false;

uploadStatus = {
	success: 0,
	invalidFileType: 1,
	error: 3
}

adjustLayout = function ()
{
	if (!adjustingLayout)
	{
		adjustingLayout = true;

		if (typeof adjustContent === 'function')
		{
			adjustContent();
		}

		if (typeof adjustLayoutContent === 'function')
		{
			adjustLayoutContent();
		}

		adjustingLayout = false;
	}
}

adjustContent = function () { }

adjustLayoutContent = function () { }

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

$(window).load(function ()
{
	adjustLayout();
});

$(window).resize(function ()
{
	adjustLayout();
});

$(window).scroll(function ()
{
	adjustLayout();
});

$.blockUI.defaults.message = null;

$.blockUI.defaults.overlayCSS = {
	backgroundColor: '#000',
	opacity: 0.0,
	cursor: 'wait'
}