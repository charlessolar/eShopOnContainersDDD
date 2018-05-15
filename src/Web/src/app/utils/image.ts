import { types } from 'mobx-state-tree';
import html2canvas from 'html2canvas';
import * as ReactCrop from 'react-image-crop';

export function fileToBase64(file: File): Promise<Data> {

  return new Promise<Data>((resolve, reject) => {
    if (!/^image\/\w+$/.test(file.type)) {
      reject('Invalid image file');
    }

    const reader = new FileReader();
    reader.onload = (e) => {
      const img = new Image();
      const fileData = (e.target as any).result;

      img.onload = () => {
        resolve({
          url: fileData,
          data: fileData.split(',')[1],
          filename: file.name,
          contentType: file.type,
          width: img.width,
          height: img.height
        });
      };

      try {
        img.src = fileData;
      } catch (e) {
        reject(e);
      }
    };
    reader.readAsDataURL(file);
  });
}

export function urlToBase64(url: string, pixelCrop?: ReactCrop.PixelCrop): Promise<Data> {
  return new Promise<Data>((resolve, reject) => {
    const canvas = document.createElement('CANVAS') as HTMLCanvasElement;
    const ctx = canvas.getContext('2d');
    const img = new Image();
    img.crossOrigin = 'Anonymous';

    img.onload = () => {
      canvas.height = pixelCrop ? pixelCrop.height : img.height;
      canvas.width = pixelCrop ? pixelCrop.width : img.width;
      if (pixelCrop) {
        ctx.drawImage(
          img,
          pixelCrop.x,
          pixelCrop.y,
          pixelCrop.width,
          pixelCrop.height,
          0,
          0,
          pixelCrop.width,
          pixelCrop.height
        );
      } else {
        ctx.drawImage(img, 0, 0);
      }

      const dataUrl = canvas.toDataURL('image/png');

      resolve({
        url: dataUrl,
        data: dataUrl.split(',')[1],
        contentType: 'image/png',
        width: canvas.width,
        height: canvas.height
      });
      // Clean up
      canvas.remove();
    };

    try {
      img.src = url;
    } catch (e) {
      reject(e);
    }
  });
}

export function elementToBase64(element: HTMLElement): Promise<Data> {
  return new Promise<Data>((resolve, reject) => {
    const img = new Image();
    img.onload = () => {
      resolve({
        url: img.src,
        data: img.src.split(',')[1],
        filename: 'generated',
        contentType: 'image/png',
        width: img.width,
        height: img.height
      });
    };

    html2canvas(element, {
      onrendered: (canvas: HTMLCanvasElement) => {
        img.src = canvas.toDataURL('image/png');
      }
    });
  });
}

export function canvasToBase64(canvas: HTMLCanvasElement): Promise<Data> {
  return new Promise<Data>((resolve, reject) => {
    const img = new Image();

    img.onload = () => {
      resolve({
        url: img.src,
        data: img.src.split(',')[1],
        filename: 'generated',
        contentType: 'image/png',
        width: img.width,
        height: img.height
      });
    };
    img.src = canvas.toDataURL('image/png');
  });
}

export interface Data {
  url?: string;
  data: string;
  filename?: string;
  contentType: string;
  height?: number;
  width?: number;
}

export const DataModel = types
  .model({
    url: types.maybe(types.string),
    data: types.string,
    filename: types.maybe(types.string),
    contentType: types.string,
    height: types.maybe(types.number),
    width: types.maybe(types.number)
  });
