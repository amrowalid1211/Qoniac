import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { currentCount: 0 };
        this.fetchNumberInWords = this.fetchNumberInWords.bind(this);
    }

    fetchNumberInWords(event) {
        const value = event.target.value;

        this.setState({
            currentCount: Number(value)
        });
        this.clearState();
        if (value.length > 0) {
            this.ConvertNumberToWords({ number: value });
        }
     
    }

    render () {
        return (
            <div>
                <input
                    type="text"
                    pattern="^\d{1,3}(,\d{3})*(\.\d{2})?$"
                    title="Please enter a valid currency amount"
                    placeholder="0,00"
                    onChange={this.fetchNumberInWords}
                />
                {this.state.error && (
                    <div className="alert alert-danger mt-3" role="alert">
                        {this.state.error}
                    </div>
                )}
                {this.state.words && (<div class="alert alert-success mt-3" role="alert">
                    {this.state.words}
                </div>)}
                
                
            </div>
            );
    }

    async ConvertNumberToWords(parameters) {
        console.log(parameters);
        const url = `qoniac/numbertowords?${new URLSearchParams(parameters).toString()}`;
        const response = await fetch(url);
        const data = await response.text();
        console.log(data);
        switch (response.status) {
            case 400:
                this.setState({ error: data, loading: false });
                break;
            default:
                this.setState({ words: data, loading: false });
                break;
        }
    }

    clearState() {
        this.setState({
            error: null,
            words: null,
            loading: false
        });
    }
}
