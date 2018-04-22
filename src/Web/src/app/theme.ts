import { createMuiTheme } from 'material-ui/styles';
import red from 'material-ui/colors/red';

const theme = createMuiTheme({
  palette: {
    primary: {
      light: '#484848 ',
      main: '#212121',
      dark: '#000000',
      contrastText: '#ffffff'
    },
    secondary: {
      light: '#ffffff ',
      main: '#f5f5f5',
      dark: '#c2c2c2',
      contrastText: '#000000'
    },
    error: red
  },
});

export default () => theme;
