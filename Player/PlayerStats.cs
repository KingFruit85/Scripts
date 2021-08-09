// using UnityEngine;

// public class PlayerStats : MonoBehaviour
// {


//     private string currentHost;

//     void Awake()
//     {
//         // Player always starts as human
//         gameObject.AddComponent<Human>();

//         currentHost = GameObject.Find("GameManager").GetComponent<GameManager>().currentHost;

//         // PlayerPrefs.SetString("currentHost", currentHost);

//         // GetComponent<Health>().currentHost = currentHost;
//     }

//     //For testing use
//     void ChangeHostBody(string newHost)
//     {

//         if (newHost != currentHost)
//         {
//             // Remove the current host script
//             switch (currentHost)
//             {
//                 default: throw new System.Exception("Unable to remove currentHost");

//                 case "Human" : Destroy(gameObject.GetComponent<Human>());break; 
//                 case "Ghost" : Destroy(gameObject.GetComponent<Ghost>());break;
//                 case "Worm"  : Destroy(gameObject.GetComponent<Worm>());break;
//             }

//             // Add new host script
//             switch (newHost)
//             {
//                 default: throw new System.Exception("newHost not recognised");

//                 case "Human" : 
//                     gameObject.AddComponent<Human>();
//                     gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Human>().idleDown);
//                     gameObject.GetComponent<PlayAnimations>().human = GetComponent<Human>();
//                     currentHost = "Human";
//                     break; 

//                 case "Ghost" : 
//                     gameObject.AddComponent<Ghost>();
//                     gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Ghost>().idleDown);
//                     gameObject.GetComponent<PlayAnimations>().ghost = GetComponent<Ghost>();
//                     currentHost = "Ghost";
//                     break;

//                 case "Worm"  : 
//                     gameObject.AddComponent<Worm>();
//                     gameObject.GetComponent<Animator>().Play(gameObject.GetComponent<Worm>().idleDown);
//                     gameObject.GetComponent<PlayAnimations>().worm = GetComponent<Worm>();
//                     currentHost = "Worm";
//                     break;
//             }
//             PlayerPrefs.SetString("currentHost", newHost);

//             gameObject.GetComponent<Health>().currentHost = newHost;

//         }    
//     }

//     void Update()
//     {



//         if (Input.GetKeyDown(KeyCode.C))
//         {
//             ChangeHostBody("Worm");
//             if (GameObject.Find("SwordAim") != null)
//             {
//                 GameObject.Find("SwordAim").SetActive(false);
//             }
//         }

//         if (Input.GetKeyDown(KeyCode.G))
//         {
//             ChangeHostBody("Ghost");
//             if (GameObject.Find("SwordAim") != null)
//             {
//                 GameObject.Find("SwordAim").SetActive(false);
//             }
//         }

//         if (Input.GetKeyDown(KeyCode.H))
//         {
//             ChangeHostBody("Human");
//             if (GameObject.Find("SwordAim") != null)
//             {
//                 GameObject.Find("SwordAim").SetActive(true);
//             }
//         }
//     }

    
// }
