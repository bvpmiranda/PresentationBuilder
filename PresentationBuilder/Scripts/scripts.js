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

		if (!window.isMobileOrTablet())
		{
			$(window).scrollTop(0);
		}

		ajustaBody();
		ajustaBarraTitulo();
		ajustaConteudo();
		ajustaRodape();

		if (typeof ajustaConteudoInterno === 'function')
		{
			ajustaConteudoInterno();
		}

		adjustingLayout = false;
	}
}

adjustBody = function ()
{

}

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