/** Global definitions for developement **/

// for style loader
declare module '*.css' {
  const styles: any;
  export = styles;
}

declare let __DEV__: boolean;
declare let API_SERVER: string;
declare let VERSION: string;
declare let COMMITHASH: string;
declare let BRANCH: string;
declare let TIMESTAMP: string;
declare let DATE: string;
declare let TIME: string;
