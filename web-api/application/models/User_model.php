<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class User_model extends CI_Model {
		public function __construct()
        {
                // Call the CI_Model constructor
                parent::__construct();
        }
        public function viewIdExists($id)
        {
        	$id = $this->db->escape($id);
        	$q = $this->db->query("SELECT 1 FROM users WHERE viewid = $id LIMIT 1;");
        	if($q->num_rows()) //broj istih viewida
        		return true;
        	return false;
        }
        public function imeiExists($id)
        {
        	$id = $this->db->escape($id);
        	$q = $this->db->query("SELECT viewid FROM users WHERE imei = $id LIMIT 1;");
        	if($q->num_rows()) //broj istih viewida
        		return $q->row_array()['viewid'];
        	return false;
        }
        public function register()
        {
        	if(isset($_POST['imei']) && !empty($_POST['imei']) && 
        	   isset($_POST['countryfrom']) && !empty($_POST['countryfrom']) &&
        	   isset($_POST['countryto']) && !empty($_POST['countryto']) &&
        	   isset($_POST['numadults']) &&
        	   isset($_POST['numchildren'])
        	)
        	{
        		$imei 			= $this->db->escape_str($_POST['imei']);
        		$countryfrom 	= $this->db->escape_str($_POST['countryfrom']);
        		$countryto 		= $this->db->escape_str($_POST['countryto']);
        		$numadults 		= $this->db->escape($_POST['numadults']);
        		$numchildren 	= $this->db->escape($_POST['numchildren']);

        		$vid 			= $countryfrom . randomNumber(6) . $countryto;	//random broj
        		while($this->viewIdExists($vid))
        			$vid = $countryfrom . randomNumber(6) . $countryto;
        		
        		//print_r($_POST);
        		//echo $vid;
        		echo "1";
        		if($this->imeiExists($imei))
        			return false;

        		//pre ovoga treba info za IMEI iz pool da li ima

        		$this->db->query("INSERT INTO users (groupid,imei,viewid,destinationcountry, origincountry, numadults, numchildren) VALUES(NULL,'$imei','$vid','$countryto','$countryfrom',$numadults,$numchildren);");
        		
        		return $vid;
        	}
        	echo "2";
        	return false;
        }
        public function checkRegister()	//vraca vid ako je auth
        {
        	if(isset($_POST['imei']) && !empty($_POST['imei']))
        	{
        		$imei 			= $this->db->escape_str($_POST['imei']);
        		$view           = $this->imeiExists($imei);
        		if($view == false)
        			return false;
        		return $view;
        	}
        	return false;
        }
        //----------------------------------------------------
        public function getUserData()	//vraca sav user data salje IMEI
        {
        	$vid = $this->checkRegister();
        	$vid = $this->db->escape($vid);
        	if(!$vid)
        		return false;
        	else
        	{
        		$q = $this->db->query("SELECT * FROM users WHERE viewid = {$vid} LIMIT 1;");
       			$row = $q->row_array();
       			return $row; //vraca user data
        	}
        }
        //----------------------------------------------------
        public function getFamilyMembers()	//vraca sve clanove porodice u grup treba IMEI
        {
        	$vid = $this->checkRegister();
        	$vid = $this->db->escape($vid);
        	if(!$vid)
        		return false;
        	else
        	{
        		$gid = $this->getUserData()['groupid'];
        		if(!$gid)
        			return false;

	        	$q = $this->db->query("SELECT * FROM groupmembers WHERE groupid = {$gid}");
	       		$alldata = $q->result_array();
	       		return $alldata; //vraca familiju
       		}
       		return false;
        }
        //----------------------------------------------------
        private function checkExistsFamilyMember($gid,$name,$male,$child)
        {
        	$gid = $this->db->escape($gid);
        	$male = $this->db->escape($male);
        	$name = $this->db->escape($name);
        	$child = $this->db->escape($child);
        	$q = $this->db->query("SELECT * FROM groupmembers WHERE groupid = {$gid} && name = {$name} && child = {$child};");
        	if($q->num_rows())
        		return true;
        	return false;
        }
        private function checkExistsFamilyMemberById($gid,$id)
        {
        	$gid = $this->db->escape($gid);
        	$id = $this->db->escape($id);
        	$q = $this->db->query("SELECT * FROM groupmembers WHERE groupid = {$gid} && id = {$id};");
        	if($q->num_rows())
        		return true;
        	return false;
        }

        private function addFamilyMember($gid,$name,$male,$child)
        {
        	$gid = $this->db->escape($gid);
        	$male = $this->db->escape($male);
        	$name = $this->db->escape($name);
        	$child = $this->db->escape($child);
        	$this->db->query("INSERT INTO groupmembers (id, groupid, name, male, child) VALUES (NULL,{$gid},{$name},{$male},{$child});");
        }
        private function modifyFamilyMember($id,$name,$male,$child)
        {
        	$id = $this->db->escape_str($id);
        	$male = $this->db->escape_str($male);
        	$name = $this->db->escape_str($name);
        	$child = $this->db->escape_str($child);
        	$q = $this->db->query("SELECT * FROM groupmembers WHERE id = '{$id}' LIMIT 1;");
        	if(!$q->num_rows())
        		return false;

        	$row = $q->row_array();

        	$query  = "UPDATE groupmembers SET 1 = 1";

        	if(!empty($male) && $male != $row['male'])
        		$query .= " , male = '{$male}'";
        	if(!empty($name) && $name != $row['name'])
        		$query .= " , name = '{$name}'";
        	if(!empty($child) && $child != $row['child'])
        		$query .= " , child = '{$child}'";

        	$query .= " WHERE id = {$id}";

        	$this->db->query($query);
        	return true;
        }
        private function deleteFamilyMembers($id)
        {
        	$id = $this->db->escape_str($id);
        	$this->db->query("DELETE FROM groupmembers WHERE id = '{$id}'");
        }
        //---------------------------------------------------------
        public function addFamilyMembers()
        {
        	//print_r($_POST);
        	//echo "123";
        	if(isset($_POST['names']) && !empty($_POST['names']) && 
        		isset($_POST['genders']) && !empty($_POST['genders']) && 
        		isset($_POST['child']))
        	{
        		$gid = $this->getUserData()['groupid'];
        		if(!$gid)
        			return false;

        		$names = $this->db->escape_str($_POST['names']);
        		$genders = $this->db->escape_str($_POST['genders']);
        		$child = $this->db->escape_str($_POST['child']);
        		$names = explode(',', $names);
        		$genders = explode(',', $genders);
        		$child = explode(',', $child);
        		//print_r($names);print_r($genders);print_r($child);
        		if((count($names) == count($genders)) == count($child))
        		{
        			$cnt = 0;
        			for($i=0; $i < count($names); $i++)
        				if(!$this->checkExistsFamilyMember($gid,$names[$i],$genders[$i],$child[$i]))	///NE DIRAJ TU
        				{
        					$this->addFamilyMember($gid,$names[$i],$genders[$i],$child[$i]);
        					$cnt++;
        				}
        			return $cnt;

        		}
        		return false;
        	}
        	return false;
        }
        public function modifyFamilyMembers()	//DRUGI PUT AKO RADI VRACA INCOMPLETE VEROVATNO 
        {	
        	if(isset($_POST['id']) && !empty($_POST['id']))
 			{
 				$id    = $this->db->escape_str($_POST['id']);
	        	$names = $this->db->escape_str($_POST['names']);
	        	$genders = $this->db->escape_str($_POST['genders']);
	        	$child = $this->db->escape_str($_POST['child']);

	        	$names = explode(',', $names);
	        	$genders = explode(',', $genders);
	        	$child = explode(',', $child);
	        	$id = explode(',', $id);

	        	$gid = $this->getUserData()['groupid'];
	        		if(!$gid)
	        			return false;

	        	//if(((count($names) == count($genders)) == count($child)) == count($id))
	        	//{
	        		$cnt = 0;
	        		for($i=0; $i < count($names); $i++)
		        		if(!$this->checkExistsFamilyMemberById($gid,$id[$i]))
		        		{
		        			$this->modifyFamilyMember($id[$i],$names[$i],$genders[$i],$child[$i]); //VEROVATN
	        					$cnt++;
		        		}
	        		return $cnt;
	        	//}
	        	//return false;
 			}
        	return false;
        }
}