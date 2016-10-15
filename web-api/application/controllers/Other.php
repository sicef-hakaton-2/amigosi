<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Other extends CI_Controller {

	public function index()
	{
		
	}
	//------------------------------------
	public function getCurrencyList()
	{
		$data = $this->other_model->getCurrencyList();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	public function getCurrentWeather()
	{
		$data = $this->other_model->getCurrentWeather();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	//vraca 
	public function getQRCode()
	{
		//header("Content-Type: image/png");
		$data = $this->other_model->getQRCode();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	public function checkToken()
	{
		$data = $this->tokenauth_model->checkToken();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
}
