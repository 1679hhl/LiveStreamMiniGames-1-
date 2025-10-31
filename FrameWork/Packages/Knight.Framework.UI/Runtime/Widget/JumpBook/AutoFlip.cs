using UnityEngine;
using System.Collections;
using Knight.Core;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Book))]
    public class AutoFlip : MonoBehaviour
    {
        public FlipMode Mode;
        public float PageFlipTime = 1;
        //public float TimeBetweenPages = 1;
        public float DelayBeforeStarting = 0;
        public bool AutoStartFlip = true;
        public Book ControledBook;
        public int AnimationFramesCount = 40;
        private bool mIsFlipping = false;
        // Use this for initialization
        void Start()
        {
            if (!this.ControledBook)
                this.ControledBook = this.GetComponent<Book>();
            if (this.AutoStartFlip)
                this.StartFlipping();
            this.ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(this.PageFlipped));
            this.ControledBook.OnEndFlip += this.PageFlipEnd;
        }
         private void PageFlipped()
        {

        }

        private void PageFlipEnd()
        {
            this.mIsFlipping = false;
            this.ControledBook.Interactable = true;
        }
        public void StartFlipping()
        {
            if (this.mIsFlipping) return;
            this.ControledBook.Interactable = false;
            this.mIsFlipping = true;
            switch (this.Mode)
            {
                case FlipMode.Bottom_RightToLeft:
                    this.StartCoroutine(this.FlipToEnd(this.ControledBook.MaxPageID));
                    break;
                case FlipMode.Bottom_LeftToRight:
                    this.StartCoroutine(this.FlipToEnd(this.ControledBook.MinPageID));
                    break;
                default:
                    break;
            }

        }

        public void StartFlipping(int nPageID)
        {
            if (this.mIsFlipping) return;
            this.ControledBook.Interactable = false;
            this.mIsFlipping = true;
            this.StartCoroutine(this.FlipToEnd(nPageID));
        }
        public void FlipRightPage()
        {
            if (this.mIsFlipping) return;
            if (this.ControledBook.CurrentPage + 2 > this.ControledBook.MaxPageID) return;
            this.ControledBook.Interactable = false;
            this.mIsFlipping = true;
            this.ControledBook.OnBeginFlip?.Invoke();
            float frameTime = this.PageFlipTime / this.AnimationFramesCount;
            float xc = (this.ControledBook.EndBottomRight.x + this.ControledBook.EndBottomLeft.x) / 2;
            float xl = ((this.ControledBook.EndBottomRight.x - this.ControledBook.EndBottomLeft.x) / 2) * 0.9f;
            //float h =  ControledBook.Height * 0.5f;
            float h = Mathf.Abs(this.ControledBook.EndBottomRight.y) * 0.9f;
            float dx = (xl) * 2 / this.AnimationFramesCount;
            this.ControledBook.ShadowClipingL.gameObject.SetActive(false);
            this.ControledBook.ShadowClipingR.gameObject.SetActive(true);
            this.StartCoroutine(this.FlipRTL(xc, xl, h, frameTime, dx, false, this.ControledBook.CurrentPage + 2));
        }
        public void FlipLeftPage()
        {
            if (this.mIsFlipping) return;
            if (this.ControledBook.CurrentPage <= this.ControledBook.MinPageID) return;
            this.mIsFlipping = true;
            this.ControledBook.Interactable = false;
           this.ControledBook.OnBeginFlip?.Invoke();
            float frameTime = this.PageFlipTime / this.AnimationFramesCount;
            float xc = (this.ControledBook.EndBottomRight.x + this.ControledBook.EndBottomLeft.x) / 2;
            float xl = ((this.ControledBook.EndBottomRight.x - this.ControledBook.EndBottomLeft.x) / 2) * 0.9f;
            //float h =  ControledBook.Height * 0.5f;
            float h = Mathf.Abs(this.ControledBook.EndBottomRight.y) * 0.9f;
            float dx = (xl) * 2 / this.AnimationFramesCount;
            this.ControledBook.ShadowClipingL.gameObject.SetActive(true);
            this.ControledBook.ShadowClipingR.gameObject.SetActive(false);
            this.StartCoroutine(this.FlipLTR(xc, xl, h, frameTime, dx, false,this.ControledBook.CurrentPage - 2));
        }
        IEnumerator FlipToEnd(int nPageID)
        {
            yield return new WaitForSeconds(this.DelayBeforeStarting);
            float frameTime = this.PageFlipTime / this.AnimationFramesCount;
            float xc = (this.ControledBook.EndBottomRight.x + this.ControledBook.EndBottomLeft.x) / 2;
            float xl = ((this.ControledBook.EndBottomRight.x - this.ControledBook.EndBottomLeft.x) / 2) * 0.9f;
            //float h =  ControledBook.Height * 0.5f;
            float h = Mathf.Abs(this.ControledBook.EndBottomRight.y) * 0.9f;
            //y=-(h/(xl)^2)*(x-xc)^2          
            //               y         
            //               |          
            //               |          
            //               |          
            //_______________|_________________x         
            //              o|o             |
            //           o   |   o          |
            //         o     |     o        | h
            //        o      |      o       |
            //       o------xc-------o      -
            //               |<--xl-->
            //               |
            //               |
            float dx = (xl) * 2 / this.AnimationFramesCount;
            switch (this.Mode)
            {
                case FlipMode.Bottom_RightToLeft:
                    this.ControledBook.ShadowClipingL.gameObject.SetActive(false);
                    this.ControledBook.ShadowClipingR.gameObject.SetActive(true);
                    for (int i = 0; i < this.ControledBook.MaxPageID; i++)
                    {
                        if(this.ControledBook.CurrentPage >= nPageID)
                        {
                            break;
                        }
                        yield return this.FlipRTL(xc, xl, h, frameTime, dx, true,nPageID);
                        //yield return new WaitForSeconds(this.TimeBetweenPages);
                    }
                    break;
                case FlipMode.Bottom_LeftToRight:
                    this.ControledBook.ShadowClipingL.gameObject.SetActive(true);
                    this.ControledBook.ShadowClipingR.gameObject.SetActive(false);
                    for (int i = 0; i < this.ControledBook.MaxPageID; i++)
                    {
                        if (this.ControledBook.CurrentPage <= nPageID)
                        {
                            break;
                        }
                        yield return this.FlipLTR(xc, xl, h, frameTime, dx, true,nPageID);
                        //this.StartCoroutine();
                    }
                    break;
            }
            this.ControledBook.OnEndFlip?.Invoke();
        }
        IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx,bool bIsContinue,int nPageID)
        {
            float x = xc + xl;
            float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

            this.ControledBook.DragBottomRightPageToPoint(new Vector3(x, y, 0),nPageID);
            for (int i = 0; i < this.AnimationFramesCount; i++)
            {
                y = (-h / (xl * xl)) * (x - xc) * (x - xc);
                this.ControledBook.UpdateBookBRTLToPoint(new Vector3(x, y, 0));
                yield return new WaitForSeconds(frameTime);
                x -= dx;
            }
            yield return this.ControledBook.ReleasePage(bIsContinue);
        }
        IEnumerator FlipLTR(float xc, float xl, float h, float frameTime, float dx,bool bIsContinue,int nPageID)
        {
            float x = xc - xl;
            float y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            this.ControledBook.DragBottomLeftPageToPoint(new Vector3(x, y, 0),nPageID);
            for (int i = 0; i < this.AnimationFramesCount; i++)
            {
                y = (-h / (xl * xl)) * (x - xc) * (x - xc);
                this.ControledBook.UpdateBookBLTRToPoint(new Vector3(x, y, 0));
                yield return new WaitForSeconds(frameTime);
                x += dx;
            }
           yield return this.ControledBook.ReleasePage(bIsContinue);
        }
    }
}
