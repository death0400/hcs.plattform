import { Component, OnInit, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, FormControl } from '@angular/forms';
import { PlatformFileInfo } from '../../hcs-models/PlatformFileInfo';
import { FileManager } from '../../hcs-lib/FileManager';
import { fromEvent, combineLatest, Observable, ReplaySubject } from 'rxjs';
import { first } from 'rxjs/operators';
import { Dialog } from '../dialogs/Dialog';
import { MatSnackBar } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';
declare const jic: { compress: (src, q, f) => any };
@Component({
  selector: 'hcs-file',
  templateUrl: './file.component.html',
  styleUrls: ['./file.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => FileComponent),
    multi: true
  }]
})
export class FileComponent implements OnInit, ControlValueAccessor {

  @Input() public layout = 'text';
  @Input() public previewSize = 's';
  @Input() multiple = false;
  @Input() accept: string;
  @Input() dir: string;
  @Input() maxFileSize = 1024 * 1024 * 5;
  _value: string | string[];
  @Input() public set value(v: string | string[]) {
    this.writeValue(v);
  }
  public fileinfoMap: { [key: string]: PlatformFileInfo } = {};
  constructor(public fileManager: FileManager, public dialog: Dialog, private snackBar: MatSnackBar, private translate: TranslateService) { }



  onChange: (v) => void;
  onTouched: () => void;
  @Input() disabled = false;
  keys: string[];
  ngOnInit() {
  }
  addFile(fileInput: HTMLInputElement) {
    fileInput.value = '';
    fileInput.click();
    fromEvent(fileInput, 'change').pipe(first()).subscribe((fileEvent: Event) => {
      const target = fileEvent.target as HTMLInputElement;
      if (target.files.length) {
        const uploads: Observable<string>[] = [];
        for (let fileIndex = 0, c = target.files.length; fileIndex < c; fileIndex++) {
          const file = target.files.item(fileIndex);
          if (file.size > this.maxFileSize) {
            this.translate.get('errors.fileSize', { size: file.size / 1024, max: this.maxFileSize / 1024, name: file.name }).subscribe(x => {
              this.snackBar.open(x, undefined, { duration: 3000 });
            });
          } else {
            if (/^image\//.test(file.type) && file.type !== 'image/gif') {
              const replay = new ReplaySubject<string>();
              const reader = new FileReader();
              const image = new Image();
              image.onload = () => {
                const compressed = jic.compress(image, 80, 'jpg').src;
                this.fileManager.upload(this.dataURItoBlob(compressed), file.name.replace(/\.\w+$/, '.jpg'), this.dir).subscribe(replay);
              };
              reader.onload = () => {
                image.src = reader.result;
              };
              reader.readAsDataURL(file);
              uploads.push(replay);
            } else {
              uploads.push(this.fileManager.upload(file, file.name, this.dir));
            }
          }
        }
        if (uploads.length) {
          combineLatest(uploads).subscribe(keys => {
            if (!this.keys) {
              this.keys = [];
            }
            keys.forEach(k => this.keys.push(k));
            this.writeValue(this.keys);
            if (this.onTouched) {
              this.onTouched();
            }
          });
        }
      }
    });
  }
  dataURItoBlob(dataURI) {
    // convert base64/URLEncoded data component to raw binary data held in a string
    let byteString;
    if (dataURI.split(',')[0].indexOf('base64') >= 0) {
      byteString = atob(dataURI.split(',')[1]);
    } else {
      byteString = unescape(dataURI.split(',')[1]);
    }

    // separate out the mime component
    const mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

    // write the bytes of the string to a typed array
    const ia = new Uint8Array(byteString.length);
    for (let i = 0; i < byteString.length; i++) {
      ia[i] = byteString.charCodeAt(i);
    }

    return new Blob([ia], { type: mimeString });
  }
  itemUp(i: number) {
    const temp = this.keys[i - 1];
    this.keys[i - 1] = this.keys[i];
    this.keys[i] = temp;
    if (this.onChange) {
      this.onChange(this.keys);
    }
    if (this.onTouched) {
      this.onTouched();
    }
  }
  itemDown(i: number) {
    const temp = this.keys[i + 1];
    this.keys[i + 1] = this.keys[i];
    this.keys[i] = temp;
    if (this.onChange) {
      this.onChange(this.keys);
    }
    if (this.onTouched) {
      this.onTouched();
    }
  }
  itemDelete(i: number) {
    this.keys.splice(i, 1);
    if (this.onChange) {
      this.onChange(this.keys);
    }
    if (this.onTouched) {
      this.onTouched();
    }
  }
  writeValue(obj: string[] | string): void {
    if (obj !== this._value) {
      this._value = obj;
      if (typeof obj === 'string') {
        obj = obj.split(',').filter(x => x);
      }
      this.fileinfoMap = {};
      if (obj) {
        this.fileManager.getFileInfo(obj as string[]).subscribe(result => {
          this.fileinfoMap = result;
          if (result) {
            this.keys = (obj as string[]).filter(x => x in result);
          } else {
            this.keys = null;
          }
          if (this.onChange) {
            this.onChange(this.keys);
          }
        });
      } else {
        this.keys = undefined;
        if (this.onChange) {
          this.onChange(this.keys);
        }
      }
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = (value) => {
      if (value instanceof Array) {
        if (value.length) {
          value = value.join(',');
        } else {
          value = undefined;
        }
      }
      fn(value);
    };
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
  protected supportsMedia(mimetype, container) {
    if (!window[`supportsMedia-elem-${container}`]) {
      window[`supportsMedia-elem-${container}`] = document.createElement(container);
    }
    const elem = window[`supportsMedia-elem-${container}`];
    if (typeof elem.canPlayType === 'function') {
      const playable = elem.canPlayType(mimetype);
      if ((playable.toLowerCase() === 'maybe') || (playable.toLowerCase() === 'probably')) {
        return true;
      }
    }
    return false;
  }
  protected isImage(file: PlatformFileInfo) {
    return /^image/i.test(file.MimeType);
  }
  protected isVideo(file: PlatformFileInfo) {
    return /^video/i.test(file.MimeType) && this.supportsMedia(file.MimeType, 'video');
  }
  protected isAudio(file: PlatformFileInfo) {
    return /^audio/i.test(file.MimeType) && this.supportsMedia(file.MimeType, 'audio');
  }
  public getFileType(file: PlatformFileInfo) {
    if (this.isImage(file)) {
      return 'image';
    } else if (this.isVideo(file)) {
      return 'video';
    } else if (this.isAudio(file)) {
      return 'audio';
    }
  }
}
