<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class User extends CI_Controller {

	public function index()
	{
		
	}
	//------------------------------------
	public function setRegister() //treba IMEI
	{
		$data = $this->user_model->register();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete form'); //
	}
	public function checkRegister() //treba IMEI
	{
		$data = $this->user_model->checkRegister();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete form'); //
	}
	//------------------------------------
	public function getUserData()	//treba IMEI
	{
		$data = $this->user_model->getUserData();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); //
	}
	//------------------------------------
	public function setFamilyMembers() //treba IMEI
	{
		$data = $this->user_model->addFamilyMembers();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'nothing added'); //
	}
	public function getFamilyMembers()	//treba IMEI
	{
		$data = $this->user_model->getFamilyMembers();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	public function deleteFamilyMembers()	//treba IMEI
	{
		$data = $this->user_model->deleteFamilyMembers();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	public function patchFamilyMembers()	//treba IMEI
	{
		$data = $this->user_model->modifyFamilyMembers();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	//------------------------------------
	public function setLastLocation()
	{
		$data = $this->locations_model->setLastLocation();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	public function getLastLocation()
	{
		$data = $this->locations_model->getLastLocation();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	public function getAllLocations()
	{
		$data = $this->locations_model->getAllLocations();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	//--------------------------------------
	
}
