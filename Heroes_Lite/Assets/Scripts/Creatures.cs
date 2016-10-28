using UnityEngine;
using System.Collections;

public class Creature {

	string name;
	int attack;
	int defense;
	int ranged_Attack;
	int min_Damage;
	int max_Damage;
	int life;
	int speed;
	string skill_1;
	string skill_2;
	string skill_3;
	int cantidad;

	int positionx; 
	int positiony;

	public Creature (string name, int attack, int defense, int min_Damage, int max_Damage, int life, int speed, 
		string skill_1, string skill_2, string skill_3, int posicionx, int posiciony){
		this.name = name;
		this.attack = attack;
		this.defense = defense;
		this.min_Damage = min_Damage;
		this.max_Damage = max_Damage;
		this.life = life;
		this.speed = speed;
		this.skill_1 = skill_1;
		this.skill_2 = skill_2;
		this.skill_3 = skill_3;
		this.positionx = posicionx;
		this.positiony = posiciony;
		cantidad = 0;
	}

	public Creature (string name, int attack, int defense, int ranged_Attack, int min_Damage, int max_Damage, int life, int speed, 
		string skill_1, string skill_2, string skill_3, int posicionx, int posiciony){
		this.name = name;
		this.attack = attack;
		this.defense = defense;
		this.ranged_Attack = ranged_Attack;
		this.min_Damage = min_Damage;
		this.max_Damage = max_Damage;
		this.life = life;
		this.speed = speed;
		this.skill_1 = skill_1;
		this.skill_2 = skill_2;
		this.skill_3 = skill_3;
		this.positionx = posicionx;
		this.positiony = posiciony;
		cantidad = 0;
	}

	public string getName(){
		return name;
	}

	public int getAttack(){
		return attack;
	}

	public int getDefense(){
		return defense;
	}

	public int getRanged_Attack(){
		return ranged_Attack;
	}

	public int getMinDamage(){
		return min_Damage;
	}

	public int getMaxDamage(){
		return max_Damage;
	}

	public int getlife(){
		return life;
	}

	public int getSpeed(){
		return speed;
	}

	public string getSkill_1(){
		return skill_1;
	}

	public string getSkill_2(){
		return skill_2;
	}

	public string getSkill_3(){
		return skill_3;
	}

	public void setPosition(int x, int y){
		this.positionx = x;
		this.positiony = y;
	}

	public int getPositionx (){
		return this.positionx;
	}

	public int getPositiony (){
		return this.positiony;
	}

	public int getCantidad(){
		return cantidad;
	}
}
