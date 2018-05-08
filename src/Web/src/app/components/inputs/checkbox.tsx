import * as React from 'react';
import { observable, action, computed } from 'mobx';
import { observer } from 'mobx-react';

import { withStyles, WithStyles, StyledComponentProps } from 'material-ui/styles';
import Input, { InputLabel } from 'material-ui/Input';
import Grid from 'material-ui/Grid';
import {
  FormLabel,
  FormControl,
  FormGroup,
  FormControlLabel,
  FormHelperText,
} from 'material-ui/Form';
import Checkbox from 'material-ui/Checkbox';

interface CheckboxProps {
  id: string;
  required?: boolean;
  label: string;
  error?: any;
  value?: any;
  fieldProps?: any;
  options: { value: string, label: string }[];

  onChange?: (value: string, checked: boolean) => void;
}

const styles = theme => ({
  formControl: {
    marginLeft: theme.spacing.unit,
    marginRight: theme.spacing.unit,
    width: 400,
  },
});

@observer
class CheckboxControl extends React.Component<CheckboxProps & WithStyles<'formControl'>, {}> {

  private handleChange(e: React.ChangeEvent<any>, value: string) {
    if (this.props.onChange) {
      this.props.onChange(value, e.target.checked);
    }
  }

  public render() {
    const { id, label, required, error, value, options, classes, fieldProps } = this.props;

    return (
      <FormControl required={required} className={classes.formControl} error={error && error[id] ? true : false} aria-describedby={id + '-text'}>
        <FormLabel htmlFor={id}>{label}</FormLabel>
        <FormGroup>
          <Grid container spacing={24}>
            {options.map(option => (
              <Grid item xs={6} key={option.value}>
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={value.findIndex(v => v === option.value) !== -1}
                      onChange={(e) => this.handleChange(e, option.value)}
                      value={option.value}
                      color='primary'
                    />
                  }
                  label={option.label}
                />
              </Grid>
            ))}
          </Grid>
        </FormGroup>
        {error && error[id] ? error[id].map((e, key) => (<FormHelperText key={key} id={id + '-' + key + '-text'}>{e}</FormHelperText>)) : undefined}
      </FormControl>
    );
  }
}

export default withStyles(styles)<CheckboxProps>(CheckboxControl);
