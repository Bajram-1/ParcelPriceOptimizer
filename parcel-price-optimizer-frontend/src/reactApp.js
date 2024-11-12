import React from "react";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import { createRoot } from 'react-dom/client';
import { VueComponent } from 'vue-in-react';
import PaymentSuccess from "./components/PaymentSuccess.vue";

function AppRouter() {
    return (
        <Router>
            <Switch>
                <Route path="/payment-success" render={() => <VueComponent component={PaymentSuccess} />} />
            </Switch>
        </Router>
    );
}

const container = document.getElementById('react-root');
if (container) {
    const root = createRoot(container);
    root.render(<AppRouter />);
}