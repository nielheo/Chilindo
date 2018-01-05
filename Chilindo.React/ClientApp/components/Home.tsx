import * as React from 'react';
import { RouteComponentProps } from 'react-router';

import { Grid, Row, Col, Button } from 'react-bootstrap'
import FormTextbox from './Common/FormTextbox'
import FormDropdown from './Common/FormDropdown'

export class Home extends React.Component<RouteComponentProps<{}>, any> {
  constructor(props: any) {
    super(props)
    this.state = {
      accountNumber: '',
      action: '',
      currency: '',
      amount: 0
    }
  }

  _onAccountNumberChange = (e: any) => {
    if (this.state.accountNumber !== e.target.value) {
      this.setState({ accountNumber: e.target.value })
    }
  }

  _onActionChange = (e: any) => {
    if (this.state.action !== e.target.value) {
      this.setState({ action: e.target.value })
    }
  }

  _onCurrencyChange = (e: any) => {
    if (this.state.action !== e.target.value) {
      this.setState({ currency: e.target.value })
    }
  }

  _onAmountChange = (e: any) => {
    if (this.state.action !== e.target.value) {
      this.setState({ amount: e.target.value })
    }
  }

  public render() {
    return <Grid>
      <Row><Col md={12}>.</Col></Row>
        <Row>
          <Col md={3}>
            <FormTextbox
              label={'Account Number'}
              disabled={false}
              value={this.state.accountNumber}
              onChange={this._onAccountNumberChange}
              error={''}
            />
          </Col>
        </Row>
        <Row>
          <Col md={3}>
            <FormDropdown
            label={'Action'}
            error={''}
            value={this.state.action}
            onChange={this._onActionChange}
            disabled={false}
          >
              <option value=''>- Select action -</option>
              <option value='balance'>Check balance</option>
              <option value='deposit'>Deposit</option>
              <option value='withdraw'>Withdraw</option>
            </FormDropdown>
          </Col>
        </Row>
        {this.state.action && this.state.action !== 'balance' && <section>
          <Row>
            <Col md={3}>
              <FormDropdown
                label={'Currency'}
                error={''}
                value={this.state.action}
                onChange={this._onCurrencyChange}
                disabled={false}
              >
                <option value=''>- Select currency -</option>
                <option value='CNY'>CNY</option>
                <option value='MYR'>MYR</option>
                <option value='THB'>THB</option>
                <option value='USD'>USD</option>
                <option value='VND'>VND</option>
              </FormDropdown>
            </Col>
        </Row>
      <Row>
          <Col md={3}>
            <FormTextbox
            label={'Amount'}
            error={''}
            value={this.state.amount}
            onChange={this._onAmountChange}
            disabled={false}
          />
        </Col>
      </Row>
        </section>
        }
        {this.state.action && <Button bsStyle='primary'>Submit</Button>

        }
      </Grid>
  }
}
