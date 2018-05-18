import * as React from 'react';
import { observable, action, computed } from 'mobx';
import { observer } from 'mobx-react';

import { withStyles, WithStyles, StyledComponentProps } from '@material-ui/core/styles';
import Input from '@material-ui/core/Input';
import InputLabel from '@material-ui/core/InputLabel';
import Grid from '@material-ui/core/Grid';
import FormLabel from '@material-ui/core/FormLabel';
import FormControl from '@material-ui/core/FormControl';
import FormGroup from '@material-ui/core/FormGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import FormHelperText from '@material-ui/core/FormHelperText';
import Checkbox from '@material-ui/core/Checkbox';

interface CheckboxProps {
  id: string;
  required?: boolean;
  label: string;
  error?: any;
  value?: any;
  fieldProps?: any;
  disabled?: boolean;
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
    const { id, label, required, error, value, options, disabled, classes, fieldProps } = this.props;

    return (
      <FormControl required={required} className={classes.formControl} disabled={disabled} error={error && error[id] ? true : false} aria-describedby={id + '-text'}>
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
