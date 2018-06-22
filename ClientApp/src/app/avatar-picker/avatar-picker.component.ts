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
  //     @Inject(MAT_DIALOG_DATA) public dataSent: any) {
  //     this.cropperSettings = new CropperSettings();
  //     this.cropperSettings.width = 100;
  //     this.cropperSettings.height = 100;
  //     this.cropperSettings.croppedWidth = 100;
  //     this.cropperSettings.croppedHeight = 100;
  //     this.cropperSettings.canvasWidth = 400;
  //     this.cropperSettings.canvasHeight = 300;

  //     this.data = {};
  // }

  // onNoClick(): void {
  //     console.log("done with popup");
  //     this.dialogRef.close();
  // }

  // onDoneClick(): void {
  //     console.log("Cancelling with popup");
  //     this.dialogRef.close(this.data.image);
  // }

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
