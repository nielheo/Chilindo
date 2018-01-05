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
      amount: 0,
      clicked: false
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

  _validInput = () => {
    if (!this.state.action)
      return false

    if (this.state.action === 'balance' && !this.state.accountNumber)
      return false

    if (this.state.action !== 'balance'
      && !this.state.accountNumber
      && !this.state.currency
      && !this.state.amount
    )
      return false

    return true
  }

  _submitRequest = () => {
    return fetch('/api/account/' + this.state.accountNumber + '/balance', {
      method: 'get',
      headers: {
        'Content-Type': 'application/json',
        'Accept-Encoding': 'gzip',
      }
    }).then(res => {
      if (res) return res.json()
    }).catch(err => { })
  }

  _onButtonClicked = () => {
    if (this._validInput())
      this._submitRequest()
    else {
      if (!this.state.clicked)
        this.setState({ clicked: true })
    }
  }

  public render() {
    return <Grid>
      <Row>
        <Col md={6}>
      <Row><Col md={12}>.</Col></Row>
        <Row>
          <Col md={8}>
            <FormTextbox
              label={'Account Number'}
              disabled={false}
              value={this.state.accountNumber}
              onChange={this._onAccountNumberChange}
              error={this.state.clicked && !this.state.accountNumber ? '* required' : ''}
            />
          </Col>
        </Row>
        <Row>
          <Col md={8}>
            <FormDropdown
            label={'Action'}
                error={this.state.clicked && !this.state.action ? '* required' : ''}
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
            <Col md={8}>
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
          <Col md={8}>
            <FormTextbox
            label={'Amount'}
            error={this.state.clicked && !this.state.amount ? '* required' : ''}
            value={this.state.amount}
            onChange={this._onAmountChange}
            disabled={false}
          />
        </Col>
      </Row>
        </section>
        }
        {this.state.action && <Button bsStyle='primary' onClick={this._onButtonClicked}>Submit</Button>

        }
        </Col>
      </Row>
    </Grid>
  }
}
