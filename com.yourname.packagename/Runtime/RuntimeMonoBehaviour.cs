using System.Collections;
using UnityEngine;

namespace packagename
{
    public class RuntimeMonoBehaviour : MonoBehaviour
    {
        #region Fields and Properties



        #endregion





        #region Events


        #endregion





        #region Initialization

        private void Awake()
        {
            // Called when the script instance is being loaded
        }
    
        private void Reset()
        {
            // Called when the script is reset (Editor only)
        }
    
        private void OnEnable()
        {
            // Called when the object becomes enabled and active
        }
    
        private void Start()
        {
            // Called before the first frame update
        }
    
        #endregion
      
    


  
        #region Update Loop
    
        private void Update()
        {
            // Called once per frame
        }
    
        private void FixedUpdate()
        {
            // Called on a fixed time interval, used for physics
        }
    
        private void LateUpdate()
        {
            // Called after all Update calls
        }
    
        #endregion
        
    



        #region Rendering and Visibility
    
        private void OnBecameVisible() { }
        private void OnBecameInvisible() { }
        private void OnWillRenderObject() { }
        private void OnRenderObject() { }

        private void OnDrawGizmos() { }
        private void OnDrawGizmosSelected() { }
        
        #endregion
        
    



        #region Physics and Collisions
    
        private void OnCollisionEnter(Collision collision) { }
        private void OnCollisionStay(Collision collision) { }
        private void OnCollisionExit(Collision collision) { }
    
        private void OnTriggerEnter(Collider other) { }
        private void OnTriggerStay(Collider other) { }
        private void OnTriggerExit(Collider other) { }
    
        private void OnControllerColliderHit(ControllerColliderHit hit) { }
    
        private void OnJointBreak(float breakForce) { }
    
        #endregion
        
    



        #region Application and Focus
    
        private void OnApplicationFocus(bool hasFocus) { }
    
        private void OnApplicationPause(bool pauseStatus) { }
    
        private void OnApplicationQuit() { }
    
        #endregion
        
    



        #region Disable and Destroy
    
        private void OnDisable()
        {
            // Called when the behaviour becomes disabled
        }
    
        private void OnDestroy()
        {
            // Called when the MonoBehaviour will be destroyed
        }
    
        #endregion
    
    



        #region Public API Methods

        public void Func() {  }

        #endregion
    
    



        #region Input and Event Handlers

        public void OnInput() { }
        public void OnEvent() { }

        #endregion
    
    



        #region Internal Methods and Helpers
    
        private void Help() { }
    
        #endregion
        
    



        #region Coroutines
    
        private IEnumerator ExampleCoroutine()
        {
            yield return null;
        }
    
        #endregion
    
    }
}
