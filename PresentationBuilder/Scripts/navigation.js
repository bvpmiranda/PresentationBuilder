var Navigation =
{
	div: null,

	baseUrl: null,

	title: null,

	clearDOM: function () { },

	navigate: function (url, title, replace, onSucessFunction, onErrorFunction)
	{
		var ajax = (url.indexOf('?') >= 0 ? '&' : '?') + "ajax=1";

		block(Navigation.div);

		$.ajax({
			type: "GET",
			url: ((replace !== undefined && replace === true) ? '' : Navigation.baseUrl) + url + ajax,
			cache: false
		}).success(function (data, textStatus, jqXHR)
		{
			Navigation.div.html(data);

			document.title = (title === undefined || title === null ? Navigation.title : Navigation.title + ' - ' + title.replace(Navigation.title + ' - ', ''));

			Navigation.clearOnPopState();
			if (replace !== undefined && replace === true)
			{
				window.history.replaceState((title === undefined || title === null ? Navigation.title : Navigation.title + ' - ' + title), (title === undefined || title === null ? Navigation.title : Navigation.title + ' - ' + title), url);
			}
			else
			{
				window.history.pushState((title === undefined || title === null ? Navigation.title : Navigation.title + ' - ' + title), (title === undefined || title === null ? Navigation.title : Navigation.title + ' - ' + title), Navigation.baseUrl + url);
			}
			Navigation.registerOnPopState();

			unblock(Navigation.div);

			if (typeof onSucessFunction === 'function')
			{
				onSucessFunction();
			}

		}).error(function (jqXHR, textStatus, errorThrown)
		{
			if (typeof onErrorFunction === 'function')
			{
				onErrorFunction();
			}

			unblock(Navigation.div);
		});
	},

	clearOnPopState: function ()
	{
		window.onpopstate = function (event)
		{

		};
	},

	registerOnPopState: function ()
	{
		window.onpopstate = function (e)
		{
			Navigation.navigate(document.location.pathname, e.state, true);
		};
	}
}

function blockBackSpace(event)
{
	var doPrevent = false;

	if (event.keyCode === 8)
	{
		var d = event.srcElement || event.target;

		if ((d.tagName.toUpperCase() === 'INPUT' && (d.type.toUpperCase() === 'TEXT' || d.type.toUpperCase() === 'PASSWORD' || d.type.toUpperCase() === 'FILE' || d.type.toUpperCase() === 'EMAIL')) || d.tagName.toUpperCase() === 'TEXTAREA' || (d.tagName.toUpperCase() === 'BODY' && $(d).hasClass('k-state-active')))
		{
			doPrevent = d.readOnly || d.disabled;
		}
		else
		{
			doPrevent = true;
		}
	}

	if (doPrevent)
	{
		event.preventDefault();
	}
}

$(document).unbind('keydown').bind('keydown', blockBackSpace);