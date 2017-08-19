$(document).ready(function () {
	var width = 728;
	var heigth = 850;
	var cur = 0;
	var leftMargin = 0;
	var no = $('#header ul').children('li').length;
	
	initialize();
	
	var start = setInterval(function () { next(); }, 5000);
	
	function initialize() {
		$('#header').css({ 'width': width + 'px', 'height': heigth + 'px' });
		$('#header ul').css({ 'width': (width * no) + 'px', 'height': heigth + 'px' });
		$('#header ul li').css({ 'width': width + 'px', 'height': heigth + 'px' });
		$('#button.next').css({ 'top': ((heigth / 2) - 25) + 'px' });
		$('#button1.prev').css({ 'top': ((heigth / 2) - 25) + 'px' });
	}
	
	function animate() {
		leftMargin = cur * width;
		$('#header ul').animate({ left: '-' + leftMargin + 'px' }, { duration:2000});
	}
	
	function next() {
		cur++;
		if (cur == no) cur = 0;
		animate();
	}
	
	function previous() {
		cur--;
		if (cur < 0) cur = no - 1;
		animate();
	}
	
	$('#button.next').click(function () { next(); clearInterval(start); });
	$('#button1.prev').click(function () { previous(); clearInterval(start); });
});