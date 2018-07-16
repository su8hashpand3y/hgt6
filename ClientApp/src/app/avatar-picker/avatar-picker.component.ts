import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-avatar-picker',
  templateUrl: './avatar-picker.component.html',
  styleUrls: ['./avatar-picker.component.css']
})
export class AvatarPickerComponent  {

  imageChangedEvent: any = '';
  croppedImage: any = '';

   constructor(public dialogRef: MatDialogRef<AvatarPickerComponent>){}

  fileChangeEvent(event: any): void {
    this.imageChangedEvent = event;
}
imageCropped(image: string) {
    this.croppedImage = image;
}
imageLoaded() {
    // show cropper
}
loadImageFailed() {
    // show message
}

select(){
    this.dialogRef.close(this.croppedImage);
}

cancel(){
    this.dialogRef.close();
}

}
