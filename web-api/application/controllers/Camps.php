<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Camps extends CI_Controller {

	public function index()
	{
		
	}
	//------------------------------------
	public function getCampList() //oko lokacije ili cela srbija
	{
		$data = $this->camps_model->getNearbyCamps();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	public function setCampList()
	{
		//$this->camps_model->setCamp();
	}
	//------------------------------------
	public function selectCamp()	//izaberi kamp koji korisnik bira kroz app
	{
		$data = $this->camps_model->setCampDestination();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	//----------------------!SALTER!----------------------------
	//cekiraj korisnika
	public function setInCamp()		//kada stigne u kamp podesi mu se to
	{
		$data = $this->camps_model->setInShelter();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	public function setOutCamp()		//kada stigne u kamp podesi mu se to
	{
		$data = $this->camps_model->setOutShelter();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	//dodaj kamp
	public function addCamp()
	{
		$data = $this->camps_model->addCamp();
		if($data != false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	//vraca koliko ljudi se krece ka toj lokaciji
	public function getNumberOfPeopleToCamp()
	{
		$data = $this->camps_model->getNumberOfPeopleToCamp();
		if($data !== false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	public function getNumberOfPeopleAtCamp()
	{
		$data = $this->camps_model->getNumberOfPeopleAtCamp();
		if($data !== false)
			echo response(200,'',$data);
		else
			echo response(400,'incomplete'); 
	}
	public function test()
	{
		
	}

}
